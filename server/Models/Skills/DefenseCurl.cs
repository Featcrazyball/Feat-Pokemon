using Server;
using PokemonPocket;

namespace Models;

public class DefenseCurl : Skill
{
    private DefenseCurl() { } // For EF Core
    public DefenseCurl(string PokemonId) : base("Defense Curl", "Normal", 0, -1, 40, 1, 0, 0, "The user curls up to raise its Defense stat and prevent flinching.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        if (user.DefenseStage >= 6) {
            await UserSession.SendMessageAsync($"Your {user.Name} used Defense Curl, but its Defense is already at maximum stage.");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Defense Curl, but its Defense is already at maximum stage.");
            return;
        }
        user.DefenseStage += 1;
        user.Defense = (float)(user.MaxDefense * SkillHelper.CalculateStage(user.DefenseStage));

        await UserSession.SendMessageAsync($"Your {user.Name} used Defense Curl, increasing its Defense to {user.Defense}.");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Defense Curl, increasing its Defense to {user.Defense}.");
    }
}