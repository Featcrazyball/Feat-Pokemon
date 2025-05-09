using Server;
using PokemonPocket;

namespace Models;

public class RockSlide : Skill
{
    private RockSlide() { } // For EF Core
    public RockSlide(string PokemonId) : base("Rock Slide", "Rock", 75, 0.9, 10, 0, 0, 0, "Large boulders are hurled at the foe to inflict damage. It may also make the target flinch.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Rock Slide") == false)
            return;

        // Damage calculation
        float damage = await SkillHelper.FeatCalculateDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Rock", target.Type?.Split('/') ?? Array.Empty<string>()),  
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
                
                // 30% chance to cause flinch
                if (!target.Flinch && Random.Shared.NextDouble() <= 0.3)
                {
                    causedFlinch = true;
                    target.Flinch = true;
                }

                if (causedFlinch)
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Rock Slide on {target.Name}'s Substitute, dealing {damage:F1} damage!\n{target.Name} flinched!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Rock Slide on your {target.Name}'s Substitute, dealing {damage:F1} damage!\nYour {target.Name} flinched!");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Rock Slide and broke {target.Name}'s Substitute!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Rock Slide and broke your {target.Name}'s Substitute!");
                }
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Rock Slide on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Rock Slide on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            
            // 30% chance to cause flinch
            if (!target.Flinch && Random.Shared.NextDouble() <= 0.3)
            {
                causedFlinch = true;
                target.Flinch = true;
            }
            
            if (causedFlinch)
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Rock Slide on {target.Name}, dealing {damage:F1} damage!\n{target.Name} flinched!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Rock Slide on your {target.Name}, dealing {damage:F1} damage!\nYour {target.Name} flinched!");
            }
            else
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Rock Slide on {target.Name}, dealing {damage:F1} damage!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Rock Slide on your {target.Name}, dealing {damage:F1} damage!");
            }
        }
        
    }
}