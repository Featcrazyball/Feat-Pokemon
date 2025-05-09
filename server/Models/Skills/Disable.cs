using Server;
using PokemonPocket;

namespace Models;

public class Disable : Skill
{
    private Disable() { } // For EF Core
    public Disable(string PokemonId) : base("Disable", "Normal", 0, 1, 20, 1, 0, 0, "The user disables the target's last move for 5 turns.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;

        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);
            
        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Disable") == false)
            return;

        int turns;
        if (Random.Shared.NextDouble() > 6/7) {turns = 7;}
        else if (Random.Shared.NextDouble() > 5/7) {turns = 6;}
        else if (Random.Shared.NextDouble() > 4/7) {turns = 5;}
        else if (Random.Shared.NextDouble() > 3/7) {turns = 4;}
        else if (Random.Shared.NextDouble() > 2/7) {turns = 3;}
        else if (Random.Shared.NextDouble() > 1/7) {turns = 2;}
        else {turns = 1;}

        if (target.Lastmove == null) {
            await UserSession.SendMessageAsync($"Your {user.Name} used Disable, but {target.Name} has no last move to disable.");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Disable, but {target.Name} has no last move to disable.");
            return;
        } else {
            target.DisabledSkill = target.Lastmove.Name ?? "Unknown";
            if (target.DisabledSkill == "Unknown") {
                await UserSession.SendMessageAsync($"Your {user.Name} used Disable, but the last move is unknown.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Disable, but the last move is unknown.");
                return;
            }
            target.DisableTurns = turns;
        }

        await UserSession.SendMessageAsync($"Your {user.Name} used Disable on {target.Name}! It will disable its last move for {turns} turn(s).");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Disable on your {target.Name}! It will disable its last move for {turns} turn(s).");
    }
}