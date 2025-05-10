using Server;
using PokemonPocket;

namespace Models;

public class SolarBeam : Skill
{
    private SolarBeam() { } // For EF Core
    public SolarBeam(string PokemonId) : base("Solar Beam", "Grass", 120, 1, 10, 1, 0, 0, "A two-turn attack. The user gathers light, then blasts a bundled beam on the next turn.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Charging turn logic
        if (user.ChargingSolar == false)
        {
            user.ChargingSolar = true;
            
            await UserSession.SendMessageAsync($"Your {user.Name} is taking in sunlight!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} is taking in sunlight!");
            return;
        }

        // Reset the charging state
        user.ChargingSolar = false;
        
        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Solar Beam") == false)
            return;

        // Damage calculation
        float damage = await SkillHelper.FeatCalculateSpecialDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Grass", target.Type?.Split('/') ?? Array.Empty<string>()),  
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
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Solar Beam and broke {target.Name}'s Substitute!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Solar Beam and broke your {target.Name}'s Substitute!");
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Solar Beam on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Solar Beam on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            await UserSession.SendMessageAsync($"Your {user.Name} used Solar Beam on {target.Name}, dealing {damage:F1} damage!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Solar Beam on your {target.Name}, dealing {damage:F1} damage!");
        }
    }
}