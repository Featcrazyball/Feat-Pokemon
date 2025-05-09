using Server;
using PokemonPocket;

namespace Models;

public class RazorWind : Skill
{
    private RazorWind() { } // For EF Core
    public RazorWind(string PokemonId) : base("Razor Wind", "Normal", 80, 1, 10, 0, 0, 0, "A two-turn attack. Blades of wind hit the foe on the second turn. It has a high critical-hit ratio.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Check if this is the first turn
        if (!user.RazorWindActive)
        {
            user.RazorWindActive = true;
            
            await UserSession.SendMessageAsync($"Your {user.Name} is whipping up a whirlwind!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} is whipping up a whirlwind!");
            return;
        }
        
        // Second turn, reset state
        user.RazorWindActive = false;

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Razor Wind") == false)
            return;

        float oldCritRate = user.CritRate;
        user.CritRate *= 3; 

        // Damage calculation
        float damage = await SkillHelper.FeatCalculateDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Normal", target.Type?.Split('/') ?? Array.Empty<string>()),  
            UserSession, 
            TargetSession
        );

        // Restore normal critical hit rate
        user.CritRate = oldCritRate;
        
        // Substitute handling
        if (target.Substitude)
        {
            if (target.SubstituteHealth <= damage)
            {
                target.Substitude = false;
                target.SubstituteHealth = 0;
                
                await UserSession.SendMessageAsync($"Your {user.Name}'s Razor Wind broke {target.Name}'s Substitute!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name}'s Razor Wind broke your {target.Name}'s Substitute!");
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name}'s Razor Wind hit {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name}'s Razor Wind hit your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            await UserSession.SendMessageAsync($"Your {user.Name}'s Razor Wind hit {target.Name}, dealing {damage:F1} damage!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name}'s Razor Wind hit your {target.Name}, dealing {damage:F1} damage!");
        }
        
    }
}