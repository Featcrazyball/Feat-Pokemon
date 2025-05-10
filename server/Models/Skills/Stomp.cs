using Server;
using PokemonPocket;

namespace Models;

public class Stomp : Skill
{
    private Stomp() { } // For EF Core
    public Stomp(string PokemonId) : base("Stomp", "Normal", 65, 1, 20, 1, 0, 0, "The target is stomped with a big foot. It may also make the target flinch.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Stomp") == false)
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

        bool causedFlinch = false;
        
        // Substitute handling
        if (target.Substitude)
        {
            if (target.SubstituteHealth <= damage)
            {
                target.Substitude = false;
                target.SubstituteHealth = 0;
                
                // 30% chance to cause flinch
                if (Random.Shared.NextDouble() <= 0.3)
                {
                    causedFlinch = true;
                    target.Flinch = true;
                }

                if (causedFlinch)
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Stomp and broke {target.Name}'s Substitute.\n{TargetSession.Username}'s {target.Name} flinched!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Stomp and broke your {target.Name}'s Substitute.\nYour {target.Name} flinched!");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Stomp and broke {target.Name}'s Substitute!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Stomp and broke your {target.Name}'s Substitute!");
                }
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Stomp on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Stomp on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            // 30% chance to cause flinch
            if (Random.Shared.NextDouble() <= 0.3)
            {
                causedFlinch = true;
                target.Flinch = true;
            }

            if (causedFlinch)
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Stomp on {target.Name}, dealing {damage:F1} damage.\n{TargetSession.Username}'s {target.Name} flinched!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Stomp on your {target.Name}, dealing {damage:F1} damage.\nYour {target.Name} flinched!");
            }
            else
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Stomp on {target.Name}, dealing {damage:F1} damage!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Stomp on your {target.Name}, dealing {damage:F1} damage!");
            }
        }
    }
}