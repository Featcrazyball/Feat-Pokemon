using Server;
using PokemonPocket;

namespace Models;

public class Thrash : Skill
{
    private Thrash() { } // For EF Core
    public Thrash(string PokemonId) : base("Thrash", "Normal", 120, 1, 10, 1, 0, 0, "The user rampages and attacks for two to three turns before getting confused.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // First turn of Thrash
        if (!user.Thrashing)
        {
            user.Thrashing = true;
            
            // Calculate number of turns (2-3)
            user.ThrashTurns = Random.Shared.NextDouble() < 0.5 ? 2 : 3;
            
            await UserSession.SendMessageAsync($"Your {user.Name} began to thrash about!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} began to thrash about!");
        }
        
        // Damage calculation
        float damage = await SkillHelper.FeatCalculateDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Normal", target.Type?.Split('/') ?? Array.Empty<string>()),  
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
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Thrash and broke {target.Name}'s Substitute!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Thrash and broke your {target.Name}'s Substitute!");
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Thrash on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Thrash on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            await UserSession.SendMessageAsync($"Your {user.Name} used Thrash on {target.Name}, dealing {damage:F1} damage!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Thrash on your {target.Name}, dealing {damage:F1} damage!");
        }
        
        user.ThrashTurns--;
        
        // If finished, user becomes confused
        if (user.ThrashTurns <= 0 && user.Thrashing == true)
        {
            // Calculate confusion duration (2-5 turns)
            int confusionTurns;
            double randomValue = Random.Shared.NextDouble();
            
            if (randomValue < 0.375) confusionTurns = 2;
            else if (randomValue < 0.75) confusionTurns = 3;
            else if (randomValue < 0.875) confusionTurns = 4;
            else confusionTurns = 5;

            user.Confused = true;
            user.ConfusionTurns = confusionTurns;
            user.Thrashing = false;
            
            await UserSession.SendMessageAsync($"Your {user.Name} became confused from fatigue!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} became confused from fatigue!");
        }
    }
}