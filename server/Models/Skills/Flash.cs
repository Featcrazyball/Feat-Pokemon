using Server;
using PokemonPocket;

namespace Models;

public class Flash : Skill
{
    private Flash() { } // For EF Core
    public Flash(string PokemonId) : base("Flash", "Normal", 0, 1, 20, 1, 0, 0, "The user flashes a bright light that cuts the target's accuracy.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Flash") == false)
            return;

        // Check if accuracy can be lowered further
        if (target.AccuracyStage <= -6)
        {
            await UserSession.SendMessageAsync($"{target.Name}'s accuracy won't go any lower!");
            await TargetSession.SendMessageAsync($"Your {target.Name}'s accuracy won't go any lower!");
            return;
        }

        // Lower target's accuracy
        target.AccuracyStage -= 1;
        
        await UserSession.SendMessageAsync($"Your {user.Name} used Flash! {target.Name}'s accuracy fell to {target.AccuracyStage}th Stage!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Flash! Your {target.Name}'s accuracy fell to {target.AccuracyStage}th Stage!");
    }
}