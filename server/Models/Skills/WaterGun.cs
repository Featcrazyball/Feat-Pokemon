using Server;
using PokemonPocket;

namespace Models;

public class WaterGun : Skill
{
    private WaterGun() { } // For EF Core
    public WaterGun(string PokemonId) : base("Water Gun", "Water", 40, 1, 25, 1, 0, 0, "The target is blasted with a forceful shot of water.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Water Gun") == false)
            return;

        // Damage calculation
        float damage = await SkillHelper.FeatCalculateSpecialDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Water", target.Type?.Split('/') ?? Array.Empty<string>()),  
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
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Water Gun and broke {target.Name}'s Substitute!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Water Gun and broke your {target.Name}'s Substitute!");
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Water Gun on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Water Gun on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            await UserSession.SendMessageAsync($"Your {user.Name} used Water Gun on {target.Name}, dealing {damage:F1} damage!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Water Gun on your {target.Name}, dealing {damage:F1} damage!");
        }
    }
}