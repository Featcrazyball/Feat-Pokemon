using Server;
using PokemonPocket;

namespace Models;

public class SkullBash : Skill
{
    private SkullBash() { } // For EF Core
    public SkullBash(string PokemonId) : base("Skull Bash", "Normal", 130, 1, 10, 1, 0, 0, "The user tucks in its head to raise its Defense in the first turn, then rams the target on the next turn.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Charging turn logic
        if (user.ChargingSkull == false)
        {
            user.ChargingSkull = true;
            
            if (user.DefenseStage < 6)
            {
                user.DefenseStage += 1;
                user.Defense = (float)(user.MaxDefense * SkillHelper.CalculateStage(user.DefenseStage));
            }
            
            await UserSession.SendMessageAsync($"Your {user.Name} tucked in its head to raise its Defense to {user.Defense:F1}!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} tucked in its head!");
            return;
        }

        // Reset the charging state
        user.ChargingSkull = false;
        
        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Skull Bash") == false)
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
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Skull Bash and broke {target.Name}'s Substitute!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Skull Bash and broke your {target.Name}'s Substitute!");
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Skull Bash on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Skull Bash on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            await UserSession.SendMessageAsync($"Your {user.Name} used Skull Bash on {target.Name}, dealing {damage:F1} damage!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Skull Bash on your {target.Name}, dealing {damage:F1} damage!");
        }
    }
}