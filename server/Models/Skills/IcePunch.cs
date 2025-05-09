using Server;
using PokemonPocket;

namespace Models;

public class IcePunch : Skill
{
    private IcePunch() { } // For EF Core
    public IcePunch(string PokemonId) : base("Ice Punch", "Ice", 75, 1, 15, 1, 0, 0, "The target is punched with an icy fist. It may also leave the target frozen.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Ice Punch") == false)
            return;

        // Damage calculation
        float damage = await SkillHelper.FeatCalculateDamage(
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
                    
                    await UserSession.SendMessageAsync($"Your {user.Name} used Ice Punch and broke {target.Name}'s Substitute and freezing it solid!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Ice Punch and broke your {target.Name}'s Substitute and freezing it solid!");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Ice Punch and broke {target.Name}'s Substitute!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Ice Punch and broke your {target.Name}'s Substitute!");
                }
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Ice Punch on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Ice Punch on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
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
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Ice Punch on {target.Name}, dealing {damage:F1} damage and freezing it solid!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Ice Punch on your {target.Name}, dealing {damage:F1} damage and freezing it solid!");
            }
            else
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Ice Punch on {target.Name}, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Ice Punch on your {target.Name}, dealing {damage:F1} damage.");
            }
        }
    }
}