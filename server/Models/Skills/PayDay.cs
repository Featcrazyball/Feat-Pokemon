using Server;
using PokemonPocket;

namespace Models;

public class PayDay : Skill
{
    private PayDay() { } // For EF Core
    public PayDay(string PokemonId) : base("Pay Day", "Normal", 40, 1, 20, 1, 0, 0, "Numerous coins are hurled at the target to inflict damage. Money is earned after the battle.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        user.PayDay += Random.Shared.Next(1, 10); 

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Pay Day") == false)
            return;

        // Damage calculation
        float damage = await SkillHelper.FeatCalculateDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Normal", target.Type?.Split('/') ?? Array.Empty<string>()),  
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
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Pay Day and broke {target.Name}'s Substitute! Coins scattered everywhere!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Pay Day and broke your {target.Name}'s Substitute! Coins scattered everywhere!");
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Pay Day on {target.Name}'s Substitute, dealing {damage:F1} damage. Coins scattered everywhere!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Pay Day on your {target.Name}'s Substitute, dealing {damage:F1} damage. Coins scattered everywhere!");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            await UserSession.SendMessageAsync($"Your {user.Name} used Pay Day on {target.Name}, dealing {damage:F1} damage! Coins scattered everywhere!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Pay Day on your {target.Name}, dealing {damage:F1} damage! Coins scattered everywhere!");
        }
        
    }
}