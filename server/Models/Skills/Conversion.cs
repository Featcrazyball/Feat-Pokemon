using Server;
using PokemonPocket;

namespace Models;

public class Conversion : Skill
{
    private Conversion() { } // For EF Core
    public Conversion(string PokemonId) : base("Conversion", "Normal", 0, 1, 30, 1, 0, 0, "The user changes its type to become the same type as the target.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;

        string oldType = user.Type ?? "Unknown";

        if (user.Firstmove == null) 
        {
            user.Type = user.Skills.ToList()[Random.Shared.Next(0, user.Skills.Count)].Type;
        }
        else 
        {
            user.Type = user.Firstmove.Type;
        }

        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        await UserSession.SendMessageAsync($"Your {user.Name} used Conversion and changed from {oldType}-type to {user.Type}-type!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Conversion and changed from {oldType}-type to {user.Type}-type!");
    }
}