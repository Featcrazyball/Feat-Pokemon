using Server;
using PokemonPocket;

namespace Models;

public class SelfDestruct : Skill
{
    private SelfDestruct() { } // For EF Core
    public SelfDestruct(string PokemonId) : base("Self-Destruct", "Normal", 200, 1, 5, 1, 0, 0, "The user attacks everything around it by causing an explosion, then faints.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // User faints after using Self-Destruct
        user.Health = 0;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Self-Destruct") == false)
            return;

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
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Self-Destruct and broke {target.Name}'s Substitute!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Self-Destruct and broke your {target.Name}'s Substitute!");
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Self-Destruct on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Self-Destruct on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            await UserSession.SendMessageAsync($"Your {user.Name} used Self-Destruct on {target.Name}, dealing {damage:F1} damage.");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Self-Destruct on your {target.Name}, dealing {damage:F1} damage.");
        }
        
        await UserSession.SendMessageAsync($"Your {user.Name} fainted from using Self-Destruct!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} fainted from using Self-Destruct!");
    }
}