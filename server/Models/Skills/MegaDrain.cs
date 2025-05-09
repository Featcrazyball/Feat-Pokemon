using Server;
using PokemonPocket;

namespace Models;

public class MegaDrain : Skill
{
    private MegaDrain() { } // For EF Core
    public MegaDrain(string PokemonId) : base("Mega Drain", "Grass", 40, 1, 15, 1, 0, 0, "A nutrient-draining attack. The user's HP is restored by half the damage taken by the target.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Mega Drain") == false)
            return;

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
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Mega Drain and broke {target.Name}'s Substitute!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Mega Drain and broke your {target.Name}'s Substitute!");
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Mega Drain on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Mega Drain on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
            
            return;
        }
        
        // Apply damage and recover HP
        float recovery;
        if (target.Health < damage) 
        {
            recovery = target.Health / 2;
            user.Health += target.Health / 2;
            if (user.Health > user.MaxHealth) { user.Health = user.MaxHealth; }
        }
        else
        {
            recovery = damage / 2;
            user.Health += damage / 2;
            if (user.Health > user.MaxHealth) { user.Health = user.MaxHealth; }
        }

        target.Health -= damage;
        await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
        await UserSession.SendMessageAsync($"Your {user.Name} used Mega Drain on {target.Name}, dealing {damage:F1} damage and recovering {recovery:F1} HP!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Mega Drain on your {target.Name}, dealing {damage:F1} damage and recovering {recovery:F1} HP!");
    }
}