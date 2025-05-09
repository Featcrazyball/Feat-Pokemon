using Server;
using PokemonPocket;

namespace Models;

public class Lick : Skill
{
    private Lick() { } // For EF Core
    public Lick(string PokemonId) : base("Lick", "Ghost", 30, 1, 30, 1, 0, 0, "The target is licked with a long tongue, causing damage. It may also leave the target with paralysis.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Lick") == false)
            return;

        // Damage calculation
        float damage = await SkillHelper.FeatCalculateDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Ghost", target.Type?.Split('/') ?? Array.Empty<string>()),  
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
                
                // 30% chance to paralyze
                if (!target.Paralyzed && Random.Shared.NextDouble() <= 0.3)
                {
                    target.Paralyzed = true;
                    if (!target.ParalyzeSpeed)
                    {
                        target.ParalyzeSpeed = true;
                        target.Speed *= 0.5f; // Speed is reduced to 25% in Gen 1
                    }
                    
                    await UserSession.SendMessageAsync($"Your {user.Name} used Lick and broke {target.Name}'s Substitute and paralyzed it!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Lick and broke your {target.Name}'s Substitute and paralyzed it!");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Lick and broke {target.Name}'s Substitute!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Lick and broke your {target.Name}'s Substitute!");
                }
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Lick on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Lick on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            // 30% chance to paralyze if target doesn't already have a status condition
            if (!target.Paralyzed && Random.Shared.NextDouble() <= 0.3)
            {
                target.Paralyzed = true;
                if (!target.ParalyzeSpeed)
                {
                    target.ParalyzeSpeed = true;
                    target.Speed *= 0.5f; // Speed is reduced to 25% in Gen 1
                }
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Lick on {target.Name}, dealing {damage:F1} damage and paralyzing it!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Lick on your {target.Name}, dealing {damage:F1} damage and paralyzing it!");
            }
            else
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Lick on {target.Name}, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Lick on your {target.Name}, dealing {damage:F1} damage.");
            }
        }
        
    }
}