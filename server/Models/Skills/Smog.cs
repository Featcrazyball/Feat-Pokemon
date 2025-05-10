using Server;
using PokemonPocket;

namespace Models;

public class Smog : Skill
{
    private Smog() { } // For EF Core
    public Smog(string PokemonId) : base("Smog", "Poison", 30, 0.7, 20, 1, 0, 0, "The target is attacked with a discharge of filthy gases. This may also poison the target.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Smog") == false)
            return;

        // Damage calculation
        float damage = await SkillHelper.FeatCalculateSpecialDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Poison", target.Type?.Split('/') ?? Array.Empty<string>()),  
            UserSession, 
            TargetSession
        );
        
        bool causedPoison = false;
        
        // Substitute handling
        if (target.Substitude)
        {
            if (target.SubstituteHealth <= damage)
            {
                target.Substitude = false;
                target.SubstituteHealth = 0;
                
                // 40% chance to poison in Gen 1
                if (!target.Poisoned && Random.Shared.NextDouble() <= 0.4 && !target.Type!.Contains("Poison"))
                {
                    target.Poisoned = true;
                    causedPoison = true;
                }

                if (causedPoison)
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Smog and broke {target.Name}'s Substitute, poisoning it!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Smog and broke your {target.Name}'s Substitute, poisoning it!");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Smog and broke {target.Name}'s Substitute!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Smog and broke your {target.Name}'s Substitute!");
                }

            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Smog on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Smog on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            // 40% chance to poison in Gen 1
            if (!target.Poisoned && Random.Shared.NextDouble() <= 0.4 && !target.Type!.Contains("Poison"))
            {
                target.Poisoned = true;
                causedPoison = true;
            }
            
            if (causedPoison)
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Smog on {target.Name}, dealing {damage:F1} damage and poisoning it!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Smog on your {target.Name}, dealing {damage:F1} damage and poisoning it!");
            }
            else
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Smog on {target.Name}, dealing {damage:F1} damage!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Smog on your {target.Name}, dealing {damage:F1} damage!");
            }
        }
    }
}