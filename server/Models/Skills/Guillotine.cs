using Server;
using PokemonPocket;

namespace Models;

public class Guillotine : Skill
{
    private Guillotine() { } // For EF Core
    public Guillotine(string PokemonId) : base("Guillotine", "Normal", 0, 0.3, 5, 1, 0, 0, "A vicious, tearing attack with pincers. The target will faint instantly if this attack hits.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // One-hit KO moves can't hit a higher-level target 
        if (target.Level > user.Level)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Guillotine, but it failed against the higher-level {target.Name}!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Guillotine, but it failed against your higher-level {target.Name}!");
            return;
        }
        
        // Check Accuracy 
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Guillotine") == false)
            return;

        // Substitute handling 
        if (target.Substitude)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Guillotine, but it failed against {target.Name}'s Substitute!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Guillotine, but it failed against your {target.Name}'s Substitute!");
            return;
        }
        
        // OHKO effect
        target.Health = 0;
        
        await UserSession.SendMessageAsync($"Your {user.Name} used Guillotine! {target.Name} was knocked out in one hit!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Guillotine! Your {target.Name} was knocked out in one hit!");
    }
}