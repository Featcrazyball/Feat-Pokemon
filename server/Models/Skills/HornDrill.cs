using Server;
using PokemonPocket;

namespace Models;

public class HornDrill : Skill
{
    private HornDrill() { } // For EF Core
    public HornDrill(string PokemonId) : base("Horn Drill", "Normal", 0, 0.3, 5, 1, 0, 0, "The user stabs the target with a horn that rotates like a drill. The target faints instantly if this attack hits.", PokemonId)    
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
            await UserSession.SendMessageAsync($"Your {user.Name} used Horn Drill, but it failed against the higher-level {target.Name}!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Horn Drill, but it failed against your higher-level {target.Name}!");
            return;
        }
        
        // Check Accuracy
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Horn Drill") == false)
            return;

        // Substitute handling - OHKO moves can't break substitutes 
        if (target.Substitude)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Horn Drill, but it failed against {target.Name}'s Substitute!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Horn Drill, but it failed against your {target.Name}'s Substitute!");
            return;
        }
        
        // OHKO effect
        target.Health = 0;
        
        await UserSession.SendMessageAsync($"Your {user.Name} used Horn Drill! {target.Name} was knocked out in one hit!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Horn Drill! Your {target.Name} was knocked out in one hit!");
    }
}