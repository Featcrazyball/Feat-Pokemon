using Server;
using PokemonPocket;

namespace Models;

public class Withdraw : Skill
{
    private Withdraw() { } // For EF Core
    public Withdraw(string PokemonId) : base("Withdraw", "Water", 0, 1, 40, 1, 0, 0, "The user withdraws its body into its hard shell, raising its Defense stat.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Check if Defense can be raised further
        if (user.DefenseStage >= 6) {
            await UserSession.SendMessageAsync($"Your {user.Name} used Withdraw, but its Defense won't go any higher!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Withdraw, but its Defense won't go any higher!");
            return;
        }
        
        // Raise Defense by 1 stage
        user.DefenseStage += 1;
        user.Defense = (float)(user.MaxDefense * SkillHelper.CalculateStage(user.DefenseStage));

        await UserSession.SendMessageAsync($"Your {user.Name} used Withdraw, raising its Defense to {user.Defense:F1}!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Withdraw, raising its Defense!");
    }
}