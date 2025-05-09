using Server;
using PokemonPocket;

namespace Models;

public class Mist : Skill
{
    private Mist() { } // For EF Core
    public Mist(string PokemonId) : base("Mist", "Ice", 0, -1, 30, 1, 0, 0, "The user cloaks itself in a white mist that prevents any of its stats from being lowered for five turns.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        user.Mist = true;
        user.MistTurns = 5;

        await UserSession.SendMessageAsync($"Your {user.Name} used Mist! It will prevent any of its stats from being lowered for 5 turns.");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Mist! It will prevent any of its stats from being lowered for 5 turns.");
    }
}