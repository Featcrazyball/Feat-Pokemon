using Server;
using PokemonPocket;

namespace Models;

public class Confusion : Skill
{
    private Confusion() { } // For EF Core
    public Confusion(string PokemonId) : base("Confusion", "Psychic", 50, 1, 25, 1, 0, 0, "The target is hit by a weak telekinetic force. It may also leave the target confused.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Confusion") == false)
            return;

        bool confuse = Random.Shared.NextDouble() <= 0.10; // 10% chance to confuse
        
        // Damage Calculation
        float damage = await SkillHelper.FeatCalculateSpecialDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Psychic", target.Type?.Split('/') ?? Array.Empty<string>()),  
            UserSession, 
            TargetSession
        );

        if (target.Substitude)
        {
            if (target.SubstituteHealth <= damage)
            {
                target.Substitude = false;
                target.SubstituteHealth = 0;
                
                if (confuse && !target.Confused)
                {
                    target.Confused = true;
                    target.ConfusionTurns = Random.Shared.Next(2, 6);
                    
                    await UserSession.SendMessageAsync($"Your {user.Name} used Confusion and broke {target.Name}'s Substitute! {target.Name} became confused!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Confusion and broke your {target.Name}'s Substitute! Your {target.Name} became confused!");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Confusion and broke {target.Name}'s Substitute!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Confusion and broke your {target.Name}'s Substitute!");
                }
            }
            else
            {
                target.SubstituteHealth -= damage;
                await UserSession.SendMessageAsync($"Your {user.Name} used Confusion on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Confusion on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            if (confuse && !target.Confused)
            {
                target.Confused = true;
                target.ConfusionTurns = Random.Shared.Next(2, 6);
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Confusion on {target.Name}, dealing {damage:F1} damage! {target.Name} became confused!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Confusion on your {target.Name}, dealing {damage:F1} damage! Your {target.Name} became confused!");
            }
            else
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Confusion on {target.Name}, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Confusion on your {target.Name}, dealing {damage:F1} damage.");
            }
        }

    }
}