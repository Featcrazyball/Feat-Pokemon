using Server;
using PokemonPocket;

namespace Models;

public class IceBeam : Skill
{
    private IceBeam() { } // For EF Core
    public IceBeam(string PokemonId) : base("Ice Beam", "Ice", 90, 1, 10, 1, 0, 0, "The target is struck with an icy-cold beam of energy. It may also freeze the target solid.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Ice Beam") == false)
            return;

        // Damage calculation
        float damage = await SkillHelper.FeatCalculateSpecialDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Ice", target.Type?.Split('/') ?? Array.Empty<string>()),  
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
                
                // 10% chance to freeze in Gen 1
                if (Random.Shared.NextDouble() <= 0.1 && !target.Freezing && !target.Burning) 
                {
                    target.Freezing = true;
                    
                    await UserSession.SendMessageAsync($"Your {user.Name} used Ice Beam and broke {target.Name}'s Substitute and freezing it solid!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Ice Beam and broke your {target.Name}'s Substitute and freezing it solid!");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Ice Beam and broke {target.Name}'s Substitute!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Ice Beam and broke your {target.Name}'s Substitute!");
                }
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Ice Beam on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Ice Beam on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            // 10% chance to freeze in Gen 1
            if (Random.Shared.NextDouble() <= 0.1 && !target.Freezing && !target.Burning) 
            {
                target.Freezing = true;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Ice Beam on {target.Name}, dealing {damage:F1} damage and freezing it solid!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Ice Beam on your {target.Name}, dealing {damage:F1} damage and freezing it solid!");
            }
            else
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Ice Beam on {target.Name}, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Ice Beam on your {target.Name}, dealing {damage:F1} damage.");
            }
        }
    }
}