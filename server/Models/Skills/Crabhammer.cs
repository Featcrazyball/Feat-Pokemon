using Server;
using PokemonPocket;

namespace Models;

public class Crabhammer : Skill
{
    private Crabhammer() { } // For EF Core
    public Crabhammer(string PokemonId) : base("Crabhammer", "Water", 90, 0.9, 10, 1, 0, 0, "The target is hammered with a large pincer. This move has a high critical-hit ratio.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Crabhammer") == false)
            return;

        // Crabhammer has a higher crit rate in Gen 1 (1/8 instead of 1/16)
        float originalCritRate = user.CritRate;
        user.CritRate *= 3;
        
        // Damage Calculation
        float damage = ((2 * user.Level / 5 + 2) * BasePower * user.Attack / target.Defense / 50 + 2) * await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Water", target.Type?.Split('/') ?? Array.Empty<string>());

        if (Random.Shared.NextDouble() <= user.CritRate)
        {
            await UserSession.SendMessageAsync("CRITICAL HIT!");
            await TargetSession.SendMessageAsync("CRITICAL HIT!");
            damage *= user.CritDmg;
        }        
        // Restore original crit rate
        user.CritRate = originalCritRate;
        
        if (target.Substitude)
        {
            if (target.SubstituteHealth <= damage)
            {
                target.Substitude = false;
                target.SubstituteHealth = 0;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Crabhammer and broke {target.Name}'s Substitute!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Crabhammer and broke your {target.Name}'s Substitute!");
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Crabhammer on {target.Name}'s Substitute, dealing {damage:F1} damage!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Crabhammer on your {target.Name}'s Substitute, dealing {damage:F1} damage!");
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            await UserSession.SendMessageAsync($"Your {user.Name} used Crabhammer on {target.Name}, dealing {damage:F1} damage!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Crabhammer on your {target.Name}, dealing {damage:F1} damage!");
        }


    }
}