using Server;
using PokemonPocket;

namespace Models;

public class Roar : Skill
{
    private Roar() { } // For EF Core
    public Roar(string PokemonId) : base("Roar", "Normal", 0, 1, 20, 0, -6, 0, "The target is scared off, and a different Pokémon is dragged out. In the wild, this ends the battle.", PokemonId)    
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
            await UserSession.SendMessageAsync($"Your {user.Name} used Roar, but {target.Name}'s substitute blocked it!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Roar, but your {target.Name}'s substitute blocked it!");
            return;
        }
        
        await UserSession.SendMessageAsync($"Your {user.Name} used Roar, but it failed!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Roar, but it failed!");
    }
}