using Server;
using PokemonPocket;

namespace Models;

public class Whirlwind : Skill
{
    private Whirlwind() { } // For EF Core
    public Whirlwind(string PokemonId) : base("Whirlwind", "Normal", 0, 1, 20, 1, 0, 0, "The target is blown away, to be replaced by another Pokémon in its party. In the wild, the battle ends.", PokemonId)    
    {
        this.PokemonId = PokemonId;
        Priority = -6;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // This skill is not coded here but in another function
    }
}