using Server;
using PokemonPocket;

namespace Models;

public class NightShade : Skill
{
    private NightShade() { } // For EF Core
    public NightShade(string PokemonId) : base("Night Shade", "Ghost", 0, 1, 15, 1, 0, 0, "The user makes the target see a frightening mirage. It inflicts damage equal to the user's level.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Night Shade") == false)
            return;

        // Night Shade does fixed damage equal to the user's level
        float damage = user.Level;
        
        // In Gen 1, Normal and Fighting types are immune to Ghost attacks
        if (target.Type != null && (target.Type.Contains("Normal") || target.Type.Contains("Fighting")))
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Night Shade, but it had no effect on {target.Name}!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Night Shade, but it had no effect on your {target.Name}!");
            return;
        }
        
        // Substitute handling
        if (target.Substitude)
        {
            if (target.SubstituteHealth <= damage)
            {
                target.Substitude = false;
                target.SubstituteHealth = 0;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Night Shade and broke {target.Name}'s Substitute!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Night Shade and broke your {target.Name}'s Substitute!");
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Night Shade on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Night Shade on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            await UserSession.SendMessageAsync($"Your {user.Name} used Night Shade on {target.Name}, dealing {damage:F1} damage!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Night Shade on your {target.Name}, dealing {damage:F1} damage!");
        }
    }
}