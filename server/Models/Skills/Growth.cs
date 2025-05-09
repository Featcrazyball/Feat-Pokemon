using Server;
using PokemonPocket;

namespace Models;

public class Growth : Skill
{
    private Growth() { } // For EF Core
    public Growth(string PokemonId) : base("Growth", "Normal", 0, -1, 20, 1, 0, 0, "The user absorbs light and boosts its Attack and Special Attack stats.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        for (int i = 0; i < 1; i++)
        {
            if (user.AttackStage >= 6) {
                await UserSession.SendMessageAsync($"Your {user.Name} used Growth, but its Attack stage is already at maximum.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Growth, but its Attack stage is already at maximum.");
                break;
            }
            user.AttackStage += 1;
            user.Attack = (float)(user.MaxAttack * SkillHelper.CalculateStage(user.AttackStage));
        }

        for (int i = 0; i < 1; i++)
        {
            if (user.SpecialAttackStage >= 6) {
                await UserSession.SendMessageAsync($"Your {user.Name} used Growth, but its Special Attack stage is already at maximum.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Growth, but its Special Attack stage is already at maximum.");
                break;
            }
            user.SpecialAttackStage += 1;
            user.SpecialAttack = (float)(user.MaxSpecialAttack * SkillHelper.CalculateStage(user.SpecialAttackStage));
        }
        if (user.Burning) {user.Attack *= (float)0.5;}

        await UserSession.SendMessageAsync($"Your {user.Name} used Growth, increasing its Attack to {user.Attack} and Special Attack to {user.SpecialAttack}.");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Growth, increasing its Attack to {user.Attack} and Special Attack to {user.SpecialAttack}.");
    }
}