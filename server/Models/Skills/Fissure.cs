using Server;
using PokemonPocket;

namespace Models;

public class Fissure : Skill
{
    private Fissure() { } // For EF Core
    public Fissure(string PokemonId) : base("Fissure", "Ground", 0, 0.3, 5, 1, 0, 0, "The user opens up a fissure in the ground and drops the target in. The target instantly faints if hit.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        if (target.Type != null && target.Type.Contains("Flying"))
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Fissure, but it doesn't affect {target.Name}!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Fissure, but it doesn't affect your {target.Name}!");
            return;
        }
        
        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Fissure") == false)
            return;

        // One-hit KO moves can't hit a higher-level target in Gen 1
        if (target.Level > user.Level && target.Levitate)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Fissure, but it failed against the higher-level {target.Name}!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Fissure, but it failed against your higher-level {target.Name}!");
            return;
        }

        // Substitute handling - OHKO moves can't break substitutes in Gen 1
        if (target.Substitude)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Fissure, but it failed against {target.Name}'s Substitute!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Fissure, but it failed against your {target.Name}'s Substitute!");
            return;
        }
        
        // OHKO effect
        target.Health = 0;
        await SkillHelper.ProcessRage(target, TargetSession, UserSession);
        
        await UserSession.SendMessageAsync($"Your {user.Name} used Fissure! {target.Name} was knocked out in one hit!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Fissure! Your {target.Name} was knocked out in one hit!");

    }
}