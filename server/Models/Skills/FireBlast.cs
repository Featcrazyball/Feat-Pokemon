using Server;
using PokemonPocket;

namespace Models;

public class FireBlast : Skill
{
    private FireBlast() { } // For EF Core
    public FireBlast(string PokemonId) : base("Fire Blast", "Fire", 120, 0.85, 5, 1, 0, 0, "The target is attacked with an intense blast of all-consuming fire. It may also leave the target with a burn.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Fire Blast") == false)
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
                
                // 30% chance to burn
                if (Random.Shared.NextDouble() <= 0.3 && !target.Freezing && !target.Burning) 
                {
                    target.Burning = true;
                    target.BurnDamage = target.MaxHealth / 16;
                    
                    if (!target.BurningAttack) 
                    {
                        target.Attack *= 0.5f;
                        target.BurningAttack = true;
                    }
                    
                    await UserSession.SendMessageAsync($"Your {user.Name} used Fire Blast and broke {target.Name}'s Substitute and burning it!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Fire Blast and broke your {target.Name}'s Substitute and burning it!");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Fire Blast and broke {target.Name}'s Substitute!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Fire Blast and broke your {target.Name}'s Substitute!");
                }
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Fire Blast on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Fire Blast on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            // 30% chance to burn
            if (Random.Shared.NextDouble() <= 0.3 && !target.Burning && !target.Freezing) 
            {
                target.Burning = true;
                target.BurnDamage = target.MaxHealth / 16;
                
                if (!target.BurningAttack && target.AttackStage >= -5) 
                {
                    target.Attack *= 0.5f;
                    target.BurningAttack = true;
                }
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Fire Blast on {target.Name}, dealing {damage:F1} damage and burning it!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Fire Blast on your {target.Name}, dealing {damage:F1} damage and burning it!");
            }
            else
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Fire Blast on {target.Name}, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Fire Blast on your {target.Name}, dealing {damage:F1} damage.");
            }
        }
    }
}