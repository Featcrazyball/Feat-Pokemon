using Server;
using PokemonPocket;

namespace Models;

public class PoisonSting : Skill
{
    private PoisonSting() { } // For EF Core
    public PoisonSting(string PokemonId) : base("Poison Sting", "Poison", 15, 1, 35, 1, 0, 0, "The user stabs the target with a poisonous stinger. It may also poison the target.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Poison Sting") == false)
            return;

        // Damage calculation
        float damage = await SkillHelper.FeatCalculateDamage(
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
                
                if (Random.Shared.NextDouble() < 0.3 && !target.BadlyPoisoned && !target.Poisoned && !target.Type!.Contains("Poison"))
                {
                    causedPoison = true;
                    target.Poisoned = true;
                }

                if (causedPoison)
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Poison Sting and broke {target.Name}'s Substitute, dealing {damage:F1} damage and poisoning it!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Poison Sting and broke your {target.Name}'s Substitute, dealing {damage:F1} damage and poisoning it!");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Poison Sting and broke {target.Name}'s Substitute, dealing {damage:F1} damage!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Poison Sting and broke your {target.Name}'s Substitute, dealing {damage:F1} damage!");
                }

            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Poison Sting on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Poison Sting on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
            
            return;
        }
        
        // Apply damage to target
        target.Health -= damage;
        await SkillHelper.ProcessRage(target, TargetSession, UserSession);
        
        // 30% chance to poison if target doesn't already have a status and isn't Poison-type
        if (Random.Shared.NextDouble() < 0.3 && !target.Poisoned && !target.BadlyPoisoned && !target.Type!.Contains("Poison"))
        {
            target.Poisoned = true;
            
            await UserSession.SendMessageAsync($"Your {user.Name} used Poison Sting on {target.Name}, dealing {damage:F1} damage and poisoning it!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Poison Sting on your {target.Name}, dealing {damage:F1} damage and poisoning it!");
        }
        else
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Poison Sting on {target.Name}, dealing {damage:F1} damage!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Poison Sting on your {target.Name}, dealing {damage:F1} damage!");
        }
    }
}