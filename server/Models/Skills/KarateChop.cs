using Server;
using PokemonPocket;

namespace Models;

public class KarateChop : Skill
{
    private KarateChop() { } // For EF Core
    public KarateChop(string PokemonId) : base("Karate Chop", "Fighting", 50, 1, 25, 1, 0, 0, "The target is attacked with a sharp chop. Critical hits land more easily.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Karate Chop") == false)
            return;

        // Karate Chop has a higher crit rate in Gen 1 (1/8 instead of 1/16)
        float originalCritRate = user.CritRate;
        user.CritRate *= 3;
        if (user.CritRate > 0.996f) user.CritRate = 0.996f; 
        
        // Damage Calculation
        float damage = ((2 * user.Level / 5 + 2) * BasePower * user.Attack / target.Defense / 50 + 2) * await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Fighting", target.Type?.Split('/') ?? Array.Empty<string>());

        if (Random.Shared.NextDouble() <= user.CritRate)
        {
            await UserSession.SendMessageAsync("CRITICAL HIT!");
            await TargetSession.SendMessageAsync("CRITICAL HIT!");
            damage *= user.CritDmg;
        }        
        // Restore original crit rate
        user.CritRate = originalCritRate;
        
        // Substitute handling
        if (target.Substitude)
        {
            if (target.SubstituteHealth <= damage)
            {
                target.Substitude = false;
                target.SubstituteHealth = 0;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Karate Chop and broke {target.Name}'s Substitute!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Karate Chop and broke your {target.Name}'s Substitute!");
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Karate Chop on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Karate Chop on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) {target.SubstituteHealth = 0;}
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            await UserSession.SendMessageAsync($"Your {user.Name} used Karate Chop on {target.Name}, dealing {damage:F1} damage!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Karate Chop on your {target.Name}, dealing {damage:F1} damage!");
        }
        
    }
}