using Server;
using PokemonPocket;

namespace Models;

public class Rest : Skill
{
    private Rest() { } // For EF Core
    public Rest(string PokemonId) : base("Rest", "Psychic", 0, 1, 10, 0, 0, 0, "The user sleeps for two turns. This fully restores the user's HP and heals any status conditions.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Induce sleep
        user.Sleeping = true;
        user.Rest= true;
        user.SleepTurns = 2;  
        
        await UserSession.SendMessageAsync($"Your {user.Name} used Rest and fell asleep! ");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Rest and fell asleep!");
    }
}