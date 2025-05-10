using Server;
using PokemonPocket;

namespace Models;

public class ThunderShock : Skill
{
    private ThunderShock() { } // For EF Core
    public ThunderShock(string PokemonId) : base("Thunder Shock", "Electric", 40, 1, 30, 1, 0, 0, "A jolt of electricity is hurled at the target to inflict damage. It may also leave the target with paralysis.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Thunder Shock") == false)
            return;

        // Damage calculation
        float damage = await SkillHelper.FeatCalculateSpecialDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Electric", target.Type?.Split('/') ?? Array.Empty<string>()),  
            UserSession, 
            TargetSession
        );

        bool causedParalysis = false;
        
        // Substitute handling
        if (target.Substitude)
        {
            if (target.SubstituteHealth <= damage)
            {
                target.Substitude = false;
                target.SubstituteHealth = 0;
                
                if (!target.Paralyzed && Random.Shared.NextDouble() <= 0.1)
                {
                    target.Paralyzed = true;
                    if (!target.ParalyzeSpeed)
                    {
                        target.ParalyzeSpeed = true;
                        target.Speed *= 0.5f;
                    }
                    causedParalysis = true;
                }

                if (causedParalysis)
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Thunder Shock and broke {target.Name}'s Substitute!\nIt also paralyzed {TargetSession.Username} {target.Name}!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Thunder Shock and broke your {target.Name}'s Substitute!\nYour {target.Name} is now paralyzed!");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Thunder Shock and broke {target.Name}'s Substitute!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Thunder Shock and broke your {target.Name}'s Substitute!");
                }
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Thunder Shock on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Thunder Shock on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            // 10% chance to paralyze in Gen 1
            if (!target.Paralyzed && Random.Shared.NextDouble() <= 0.1)
            {
                target.Paralyzed = true;
                if (!target.ParalyzeSpeed)
                {
                    target.ParalyzeSpeed = true;
                    target.Speed *= 0.5f;
                }
                causedParalysis = true;
            }

            if (causedParalysis)
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Thunder Shock on {target.Name}, dealing {damage:F1} damage!\nIt also paralyzed {TargetSession.Username} {target.Name}!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Thunder Shock on your {target.Name}, dealing {damage:F1} damage!\nYour {target.Name} is now paralyzed!");
            }
            else
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Thunder Shock on {target.Name}, dealing {damage:F1} damage!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Thunder Shock on your {target.Name}, dealing {damage:F1} damage!");
            }
        }
    }
}