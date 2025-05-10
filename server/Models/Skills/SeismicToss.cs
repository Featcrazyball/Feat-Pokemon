using Server;
using PokemonPocket;

namespace Models;

public class SeismicToss : Skill
{
    private SeismicToss() { } // For EF Core
    public SeismicToss(string PokemonId) : base("Seismic Toss", "Fighting", 0, 1, 20, 1, 0, 0, "The target is thrown using the power of gravity. It inflicts damage equal to the user's level.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Seismic Toss") == false)
            return;

        // Damage equal to user's level
        float damage = user.Level;
        
        // Check if target is Ghost type (immune in Gen 1)
        if (target.Type != null && target.Type.Contains("Ghost"))
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Seismic Toss, but it had no effect on {target.Name}!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Seismic Toss, but it had no effect on your {target.Name}!");
            return;
        }
        
        // Substitute handling
        if (target.Substitude)
        {
            if (target.SubstituteHealth <= damage)
            {
                target.Substitude = false;
                target.SubstituteHealth = 0;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Seismic Toss and broke {target.Name}'s Substitute!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Seismic Toss and broke your {target.Name}'s Substitute!");
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Seismic Toss on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Seismic Toss on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            await UserSession.SendMessageAsync($"Your {user.Name} used Seismic Toss on {target.Name}, dealing {damage:F1} damage!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Seismic Toss on your {target.Name}, dealing {damage:F1} damage!");
        }
    }
}