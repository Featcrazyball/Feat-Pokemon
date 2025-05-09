using Server;
using PokemonPocket;

namespace Models;

public class Barrier : Skill
{
    private Barrier() { } // For EF Core
    public Barrier(string PokemonId) : base("Barrier", "Psychic", 0, -1, 20, 1, 0, 0, "The user creates a barrier that sharply raises its Defense.", PokemonId)    
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
            if (user.DefenseStage >= 6) {break;}
            user.DefenseStage += 1;
            user.Defense = (float)(user.MaxDefense * SkillHelper.CalculateStage(user.DefenseStage));
        }

        await UserSession.SendMessageAsync($"Your {user.Name} used Barrier, increasing its Defense to {user.Defense}.");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Barrier, increasing its Defense to {user.Defense}.");
    }
}