using Server;
using PokemonPocket;

namespace Models;

public class DrillPeck : Skill
{
    private DrillPeck() { } // For EF Core
    public DrillPeck(string PokemonId) : base("Drill Peck", "Flying", 80, 1, 20, 1, 0, 0, "The user attacks the target with a sharp beak. It may also make the target flinch.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, "Drill Peck") == false)
            return;

        // Fixed damage
        float damage = await SkillHelper.FeatCalculateDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Flying", target.Type?.Split('/') ?? Array.Empty<string>()),  
            UserSession, 
            TargetSession
        );
        // Substitute
        if (target.Substitude == true)
        {
            if (target.SubstituteHealth <= damage) 
            {
                target.Substitude = false;
                target.SubstituteHealth = 0;
                await UserSession.SendMessageAsync($"Your {user.Name} used Drill Peck and broke {target.Name}'s Substitute!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Drill Peck and broke your {target.Name}'s Substitute!");
            }
            else
            {
                target.SubstituteHealth -= damage;
                await UserSession.SendMessageAsync($"Your {user.Name} used Drill Peck on {target.Name}'s Substitute, dealing {damage} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Drill Peck on your {target.Name}'s Substitute, dealing {damage} damage.");
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            await UserSession.SendMessageAsync($"Your {user.Name} used Drill Peck on {target.Name}, dealing {damage} damage.");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Drill Peck on your {target.Name}, dealing {damage} damage.");
        }

    }
}