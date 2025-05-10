using Server;
using PokemonPocket;

namespace Models;

public class Teleport : Skill
{
    private Teleport() { } // For EF Core
    public Teleport(string PokemonId) : base("Teleport", "Psychic", 0, 1, 20, 1, 0, 0, "The user teleports using telepathic power. It may fail in battles against wild Pokémon.", PokemonId)    
    {
        this.PokemonId = PokemonId;
        Priority = -6;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // In Gen 1 Teleport does nothing in battle except waste a turn
        await UserSession.SendMessageAsync($"Your {user.Name} used Teleport! But it failed!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Teleport! But it failed!");
    }
}