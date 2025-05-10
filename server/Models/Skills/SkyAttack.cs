using Server;
using PokemonPocket;

namespace Models;

public class SkyAttack : Skill
{
    private SkyAttack() { } // For EF Core
    public SkyAttack(string PokemonId) : base("Sky Attack", "Flying", 140, 0.9, 5, 1, 0, 0, "A second-turn attack move where critical hits land more easily. It may also make the target flinch.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Charging turn logic
        if (user.ChargingSky == false)
        {
            user.ChargingSky = true;
            
            await UserSession.SendMessageAsync($"Your {user.Name} is glowing with energy!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} is glowing with energy!");
            return;
        }

        // Reset the charging state
        user.ChargingSky = false;
        
        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Sky Attack") == false)
            return;

        // Sky Attack has a higher crit rate in Gen 1 
        float originalCritRate = user.CritRate;
        user.CritRate *= 3;
        if (user.CritRate > 0.996f) user.CritRate = 0.996f; 
        
        // Damage Calculation
        float damage = ((2 * user.Level / 5 + 2) * BasePower * user.Attack / target.Defense / 50 + 2) * await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Flying", target.Type?.Split('/') ?? Array.Empty<string>());

        // Critical hit check
        bool causedFlinch = false;
        if (Random.Shared.NextDouble() <= user.CritRate)
        {
            await UserSession.SendMessageAsync("CRITICAL HIT!");
            await TargetSession.SendMessageAsync("CRITICAL HIT!");
            damage *= user.CritDmg;
        }
        
        // Restore original crit rate
        user.CritRate = originalCritRate;
        
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
                    await UserSession.SendMessageAsync($"Your {user.Name} used Sky Attack and broke {target.Name}'s Substitute!\n{TargetSession.Username}'s {target.Name} flinched!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Sky Attack and broke your {target.Name}'s Substitute!\nYour {target.Name} flinched!");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Sky Attack and broke {target.Name}'s Substitute!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Sky Attack and broke your {target.Name}'s Substitute!");
                }

            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Sky Attack on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Sky Attack on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
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
                await UserSession.SendMessageAsync($"Your {user.Name} used Sky Attack on {target.Name}, dealing {damage:F1} damage.\n{TargetSession.Username}'s {target.Name} flinched!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Sky Attack on your {target.Name}, dealing {damage:F1} damage.\nYour {target.Name} flinched!");
            }
            else
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Sky Attack on {target.Name}, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Sky Attack on your {target.Name}, dealing {damage:F1} damage.");
            }
        }
    }
}