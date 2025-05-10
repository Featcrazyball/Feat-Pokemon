using Server;
using PokemonPocket;

namespace Models;

public class Smokescreen : Skill
{
    private Smokescreen() { } // For EF Core
    public Smokescreen(string PokemonId) : base("Smokescreen", "Normal", 0, 1, 20, 1, 0, 0, "The user releases an obscuring cloud of smoke or ink. It reduces the target's accuracy.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Check if substitute is present
        if (target.Substitude)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Smokescreen, but {target.Name}'s substitute blocked it!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Smokescreen, but your {target.Name}'s substitute blocked it!");
            return;
        }

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Smokescreen") == false)
            return;

        // Check if accuracy can be lowered further
        if (target.AccuracyStage <= -6) {
            await UserSession.SendMessageAsync($"Your {user.Name} used Smokescreen, but {target.Name}'s Accuracy won't go any lower!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Smokescreen, but your {target.Name}'s Accuracy won't go any lower!");
            return;
        }
        
        // Lower accuracy by 1 stage
        target.AccuracyStage -= 1;

        await UserSession.SendMessageAsync($"Your {user.Name} used Smokescreen, lowering {target.Name}'s Accuracy!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Smokescreen, lowering your {target.Name}'s Accuracy!");
    }
}