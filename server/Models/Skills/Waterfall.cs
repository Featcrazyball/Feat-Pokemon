using Server;
using PokemonPocket;

namespace Models;

public class Waterfall : Skill
{
    private Waterfall() { } // For EF Core
    public Waterfall(string PokemonId) : base("Waterfall", "Water", 80, 1, 15, 1, 0, 0, "The user charges at the target and may make it flinch. It can also be used to climb a waterfall.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Waterfall") == false)
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
        
        bool causedFlinch = false;
        
        // Substitute handling
        if (target.Substitude)
        {
            if (target.SubstituteHealth <= damage)
            {
                target.Substitude = false;
                target.SubstituteHealth = 0;
                
                // 20% chance to cause flinch (in Gen 2+, but for this implementation we'll include it)
                if (Random.Shared.NextDouble() <= 0.2)
                {
                    causedFlinch = true;
                    target.Flinch = true;
                }
                
                if (causedFlinch)
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Waterfall and broke {target.Name}'s Substitute!\n{target.Name} flinched!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Waterfall and broke your {target.Name}'s Substitute!\nYour {target.Name} flinched!");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Waterfall and broke {target.Name}'s Substitute!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Waterfall and broke your {target.Name}'s Substitute!");
                }
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Waterfall on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Waterfall on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            // 20% chance to cause flinch
            if (Random.Shared.NextDouble() <= 0.2)
            {
                causedFlinch = true;
                target.Flinch = true;
            }
            
            if (causedFlinch)
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Waterfall on {target.Name}, dealing {damage:F1} damage!\n{target.Name} flinched!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Waterfall on your {target.Name}, dealing {damage:F1} damage!\nYour {target.Name} flinched!");
            }
            else
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Waterfall on {target.Name}, dealing {damage:F1} damage!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Waterfall on your {target.Name}, dealing {damage:F1} damage!");
            }
        }
    }
}