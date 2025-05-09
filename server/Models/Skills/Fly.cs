using Server;
using PokemonPocket;

namespace Models;

public class Fly : Skill
{
    private Fly() { } // For EF Core
    public Fly(string PokemonId) : base("Fly", "Flying", 90, 0.95, 15, 1, 0, 0, "The user flies up high, then strikes on the next turn. The user becomes semi-invulnerable during flight.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        if (user.Flying == false) {user.Flying = true;}

        await UserSession.SendMessageAsync($"Your {user.Name} used Fly! It will strike on the next turn!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Fly! It will strike on the next turn!");
    }
}