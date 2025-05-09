using Server;
using PokemonPocket;

namespace Models;

public class LightScreen : Skill
{
    private LightScreen() { } // For EF Core
    public LightScreen(string PokemonId) : base("Light Screen", "Psychic", 0, 1, 30, 1, 0, 0, "A wondrous wall of light is put up to reduce damage from special attacks for five turns.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);
        
        // Check if Light Screen is already active
        if (user.LightScreen)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} already has Light Screen active!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} already has Light Screen active!");
            return;
        }
        
        // Apply Light Screen effect
        user.LightScreen = true;
        user.LightScreenTurns = 5; 
            
        await UserSession.SendMessageAsync($"Your {user.Name} used Light Screen! A wondrous wall of light was put up to reduce damage from special attacks!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Light Screen! A wondrous wall of light was put up to reduce damage from special attacks!");
    }
}