using Server;
using PokemonPocket;

namespace Models;

public class Roar : Skill
{
    private Roar() { } // For EF Core
    public Roar(string PokemonId) : base("Roar", "Normal", 0, 1, 20, 0, -6, 0, "The target is scared off, and a different Pokémon is dragged out. In the wild, this ends the battle.", PokemonId)    
    {
        this.PokemonId = PokemonId;
        Priority = 6;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // This skill is not coded here but in another function
    }
}