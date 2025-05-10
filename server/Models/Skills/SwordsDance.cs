using Server;
using PokemonPocket;

namespace Models;

public class SwordsDance : Skill
{
    private SwordsDance() { } // For EF Core
    public SwordsDance(string PokemonId) : base("Swords Dance", "Normal", 0, 1, 30, 1, 0, 0, "A frenetic dance to uplift the fighting spirit. It sharply raises the user's Attack stat.", PokemonId)    
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
            await UserSession.SendMessageAsync($"Your {user.Name} used Swords Dance, but its Attack won't go any higher!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Swords Dance, but its Attack won't go any higher!");
            return;
        }
        
        for (int i = 0; i < 2; i++)
        {
            if (user.AttackStage >= 6) {break;}
            user.AttackStage += 1;
            user.Attack = (float)(user.MaxAttack * SkillHelper.CalculateStage(user.AttackStage));
        }

        if (user.Burning) {
            user.Attack *= 0.5f;
        }

        await UserSession.SendMessageAsync($"Your {user.Name} used Swords Dance, sharply raising its Attack by 2 Stages to {user.Attack:F1}!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Swords Dance, sharply raising its Attack by 2 Stages to {user.Attack:F1}!!");
    }
}