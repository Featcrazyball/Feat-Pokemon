using Server;
using PokemonPocket;

namespace Models;

public class Ember : Skill
{
    private Ember() { } // For EF Core
    public Ember(string PokemonId) : base("Ember", "Fire", 40, 1, 25, 1, 0, 0, "The target is attacked with small flames. It may also leave the target with a burn.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Ember") == false)
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

        // 10% chance to burn
        bool burn = Random.Shared.NextDouble() <= 0.10;
        
        // Substitute handling
        if (target.Substitude)
        {
            if (target.SubstituteHealth <= damage)
            {
                target.Substitude = false;
                target.SubstituteHealth = 0;

                if (burn && !target.Freezing && !target.Burning) 
                {
                    target.Burning = true;
                    target.BurnDamage = 0.0625f * target.MaxHealth;
                }
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Ember and broke {target.Name}'s Substitute and burned {target.Name}!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Ember and broke your {target.Name}'s Substitute and burned {target.Name}!");
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Ember on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Ember on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            // Apply burn if chance succeeds
            if (burn && !target.Freezing && !target.Burning) // Cannot burn a frozen target
            {
                target.Burning = true;
                target.BurnDamage = 0.0625f * target.MaxHealth;

                
                await UserSession.SendMessageAsync($"Your {user.Name} used Ember on {target.Name}, dealing {damage:F1} damage and burning it!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Ember on your {target.Name}, dealing {damage:F1} damage and burning it!");
            }
            else
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Ember on {target.Name}, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Ember on your {target.Name}, dealing {damage:F1} damage.");
            }
        }
        
    }
}