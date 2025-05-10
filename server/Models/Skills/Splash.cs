using Server;
using PokemonPocket;

namespace Models;

public class Splash : Skill
{
    private Splash() { } // For EF Core
    public Splash(string PokemonId) : base("Splash", "Normal", 0, 1, 40, 1, 0, 0, "The user just flops and splashes around to no effect at all...", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Splash does nothing
        await UserSession.SendMessageAsync($"Your {user.Name} used Splash... but nothing happened!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Splash... but nothing happened!");
    }
}