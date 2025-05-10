using Server;
using PokemonPocket;

namespace Models;

public class Sharpen : Skill
{
    private Sharpen() { } // For EF Core
    public Sharpen(string PokemonId) : base("Sharpen", "Normal", 0, 1, 30, 1, 0, 0, "The user makes its edges more jagged, which raises its Attack stat.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Check if Attack can be raised further
        if (user.AttackStage >= 6)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Sharpen, but its Attack won't go any higher!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Sharpen, but its Attack won't go any higher!");
            return;
        }
        
        // Raise Attack by 1 stage
        user.AttackStage += 1;
        user.Attack = (float)(user.MaxAttack * SkillHelper.CalculateStage(user.AttackStage));
        if (user.Burning) {
            user.Attack *= 0.5f;
        }

        await UserSession.SendMessageAsync($"Your {user.Name} used Sharpen, raising its Attack to {user.Attack:F1}!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Sharpen, raising its Attack!");
    }
}