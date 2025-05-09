using Server;
using PokemonPocket;

namespace Models;

public class FirePunch : Skill
{
    private FirePunch() { } // For EF Core
    public FirePunch(string PokemonId) : base("Fire Punch", "Fire", 75, 1, 15, 1, 0, 0, "The target is punched with a fiery fist. It may also leave the target with a burn.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Fire Punch") == false)
            return;

        // Damage calculation
        float damage = await SkillHelper.FeatCalculateSpecialDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Fire", target.Type?.Split('/') ?? Array.Empty<string>()),  
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
                
                // 10% chance to burn in Gen 1
                if (Random.Shared.NextDouble() <= 0.1 && !target.Freezing && !target.Burning) 
                {
                    target.Burning = true;
                    target.BurnDamage = target.MaxHealth / 16;
                    
                    if (!target.BurningAttack) 
                    {
                        target.Attack *= 0.5f;
                        target.BurningAttack = true;
                    }
                    
                    await UserSession.SendMessageAsync($"Your {user.Name} used Fire Punch and broke {target.Name}'s Substitute and burning it!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Fire Punch and broke your {target.Name}'s Substitute and burning it!");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Fire Punch and broke {target.Name}'s Substitute!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Fire Punch and broke your {target.Name}'s Substitute!");
                }
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Fire Punch on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Fire Punch on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            // 10% chance to burn in Gen 1
            if (Random.Shared.NextDouble() <= 0.1 && !target.Burning && !target.Freezing) 
            {
                target.Burning = true;
                target.BurnDamage = target.MaxHealth / 16;
                
                if (!target.BurningAttack && target.AttackStage >= -5) 
                {
                    target.Attack *= 0.5f;
                    target.BurningAttack = true;
                }
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Fire Punch on {target.Name}, dealing {damage:F1} damage and burning it!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Fire Punch on your {target.Name}, dealing {damage:F1} damage and burning it!");
            }
            else
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Fire Punch on {target.Name}, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Fire Punch on your {target.Name}, dealing {damage:F1} damage.");
            }
        }
    }
}