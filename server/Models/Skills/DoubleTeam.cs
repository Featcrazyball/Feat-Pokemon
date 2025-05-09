using Server;
using PokemonPocket;

namespace Models;

public class DoubleTeam : Skill
{
    private DoubleTeam() { } // For EF Core
    public DoubleTeam(string PokemonId) : base("Double Team", "Normal", 0, -1, 15, 1, 0, 0, "The user creates a double to confuse the target.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        for (int i = 0; i < 2; i++)
        {
            if (user.EvasionStage >= 6) {break;}
            user.EvasionStage += 1;
        }

        await UserSession.SendMessageAsync($"Your {user.Name} used Double Team, increasing its Evasion to Stage {user.EvasionStage}.");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Double Team, increasing its Evasion to Stage {user.EvasionStage}.");
    }
}