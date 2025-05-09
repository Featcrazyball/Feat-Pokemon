using Server;
using PokemonPocket;

namespace Models;

public class PetalDance : Skill
{
    private PetalDance() { } // For EF Core
    public PetalDance(string PokemonId) : base("Petal Dance", "Grass", 120, 1, 10, 1, 0, 0, "The user attacks by scattering petals for 2-3 turns. The user then becomes confused.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        if (user.PetalDance == false)
        {
            user.PetalDance = true;
            user.PetalDanceTurns = Random.Shared.Next(2, 4); // 2-3 turns
        }

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Petal Dance") == false)
        {
            user.PetalDanceTurns--;
            return;
        }

        // Damage calculation
        float damage = await SkillHelper.FeatCalculateSpecialDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Grass", target.Type?.Split('/') ?? Array.Empty<string>()),  
            UserSession, 
            TargetSession
        );
        
        // Substitute handling
        if (target.Substitude)
        {
            if (target.SubstituteHealth <= damage)
            {
                target.Substitude = false;
                target.SubstituteHealth = 0;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Petal Dance and broke {target.Name}'s Substitute!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Petal Dance and broke your {target.Name}'s Substitute!");
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Petal Dance on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Petal Dance on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            await UserSession.SendMessageAsync($"Your {user.Name} used Petal Dance on {target.Name}, dealing {damage:F1} damage!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Petal Dance on your {target.Name}, dealing {damage:F1} damage!");
        }
        
        user.PetalDanceTurns--;
        
        // If finished, user becomes confused
        if (user.PetalDanceTurns <= 0 && user.PetalDance == true)
        {
            // Calculate number of turns (2-5)
            int turns;
            double hitChance = Random.Shared.NextDouble();
            
            if (hitChance < 0.375) {turns = 2;}
            else if (hitChance < 0.75) {turns = 3;}
            else if (hitChance < 0.875) {turns = 4;}
            else {turns = 5;}

            user.Confused = true;
            user.ConfusionTurns = turns; 

            user.PetalDance = false;
            
            await UserSession.SendMessageAsync($"Your {user.Name} became confused for {turns} turns due to fatigue!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} became confused for {turns} turns due to fatigue!");
        }
    }
}