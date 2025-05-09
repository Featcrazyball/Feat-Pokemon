using Server;
using PokemonPocket;

namespace Models;

public class HyperBeam : Skill
{
    private HyperBeam() { } // For EF Core
    public HyperBeam(string PokemonId) : base("Hyper Beam", "Normal", 150, 0.9, 5, 1, 0, 0, "The target is attacked with a powerful beam. The user can't move on the next turn.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Hyper Beam") == false)
            return;

        // Damage calculation
        float damage = await SkillHelper.FeatCalculateSpecialDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Normal", target.Type?.Split('/') ?? Array.Empty<string>()),  
            UserSession, 
            TargetSession
        );
        
        // Hyper Beam requires a recharge turn unless it defeats the target
        user.HyperBeamRecharge = true;
        
        // Substitute handling
        if (target.Substitude)
        {
            if (target.SubstituteHealth <= damage)
            {
                target.Substitude = false;
                target.SubstituteHealth = 0;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Hyper Beam and broke {target.Name}'s Substitute!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Hyper Beam and broke your {target.Name}'s Substitute!");
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Hyper Beam on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Hyper Beam on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            if (target.Health <= 0)
            {
                target.Health = 0;
                user.HyperBeamRecharge = false; // No recharge needed if target faints
                await UserSession.SendMessageAsync($"Your {user.Name} used Hyper Beam on {target.Name}, dealing {damage:F1} damage!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Hyper Beam on your {target.Name}, dealing {damage:F1} damage!");
            }
            else
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Hyper Beam on {target.Name}, dealing {damage:F1} damage!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Hyper Beam on your {target.Name}, dealing {damage:F1} damage!");
            }
        }
        
    }
}