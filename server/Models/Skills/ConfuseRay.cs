using Server;
using PokemonPocket;

namespace Models;

public class ConfuseRay : Skill
{
    private ConfuseRay() { } // For EF Core
    public ConfuseRay(string PokemonId) : base("Confuse Ray", "Ghost", 0, 1, 10, 1, 0, 0, "The target is exposed to a sinister ray that triggers confusion.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        if (target.Confused)
        {
            await UserSession.SendMessageAsync($"{target.Name} is already confused!");
            await TargetSession.SendMessageAsync($"Your {target.Name} is already confused!");
            return;
        }

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Confuse Ray") == false)
            return;

        if (target.Substitude)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Confuse Ray, but it failed due to {TargetSession.Username}'s {target.Name}'s Substitude!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Confuse Ray, but it failed due to your substitude!");
            return;
        }

        int hits;
        float chance = Random.Shared.Next(0, 100);
        if (chance > 87.5) hits = 5;
        else if (chance > 75) hits = 4;
        else if (chance > 37.5) hits = 3;
        else hits = 2;

        // Apply confusion
        target.Confused = true;
        target.ConfusionTurns = hits;

        await UserSession.SendMessageAsync($"Your {user.Name} used Confuse Ray on {target.Name}! {target.Name} became confused!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Confuse Ray on your {target.Name}! Your {target.Name} became confused!");
    }
}