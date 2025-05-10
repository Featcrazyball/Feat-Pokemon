using Server;
using PokemonPocket;

namespace Models;

public class Substitute : Skill
{
    private Substitute() { } // For EF Core
    public Substitute(string PokemonId) : base("Substitute", "Normal", 0, 1, 10, 1, 0, 0, "The user creates a substitute for itself using some of its HP. The substitute takes damage for the user.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Check if already has substitute
        if (user.Substitude)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} already has a substitute!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} tried to use Substitute, but it already has one!");
            return;
        }

        // Check if enough HP to create substitute (25% of max HP)
        float hpCost = user.MaxHealth / 4;
        if (user.Health <= hpCost)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} doesn't have enough HP to create a substitute!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} tried to use Substitute, but it doesn't have enough HP!");
            return;
        }

        // Create substitute
        user.Substitude = true;
        user.SubstituteHealth = hpCost;
        user.Health -= hpCost;
        
        await UserSession.SendMessageAsync($"Your {user.Name} used {hpCost:F1} HP to create a substitute!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Substitute!");
    }
}