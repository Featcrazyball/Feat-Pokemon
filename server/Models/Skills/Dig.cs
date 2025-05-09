using Server;
using PokemonPocket;

namespace Models;

public class Dig : Skill
{
    private Dig() { } // For EF Core
    public Dig(string PokemonId) : base("Dig", "Ground", 80, 1, 10, 1, 0, -5, "The user digs underground on the first turn and attacks on the second turn.", PokemonId)    
    {
        this.PokemonId = PokemonId;
        Priority = -1;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        user.Dig = true;
        user.Underground = true;

        await UserSession.SendMessageAsync($"Your {user.Name} dug underground! It will attack next turn.");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} dug underground! It will attack next turn.");
        return;
    }
}