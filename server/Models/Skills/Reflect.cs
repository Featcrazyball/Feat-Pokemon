using Server;
using PokemonPocket;

namespace Models;

public class Reflect : Skill
{
    private Reflect() { } // For EF Core
    public Reflect(string PokemonId) : base("Reflect", "Psychic", 0, 1, 20, 0, 0, 0, "A wondrous wall of light is put up to reduce damage from physical attacks for five turns.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Check if Reflect is already active
        if (user.Reflect)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Reflect, but a barrier is already in effect!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Reflect, but a barrier is already in effect!");
            return;
        }
        
        // Set Reflect status
        user.Reflect = true;
        user.ReflectTurns = 5;
        
        await UserSession.SendMessageAsync($"Your {user.Name} used Reflect! A barrier formed that halves physical damage!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Reflect! A barrier formed that halves physical damage!");
    }
}