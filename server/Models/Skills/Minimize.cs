using Server;
using PokemonPocket;

namespace Models;

public class Minimize : Skill
{
    private Minimize() { } // For EF Core
    public Minimize(string PokemonId) : base("Minimize", "Normal", 0, 1, 10, 1, 0, 0, "The user compresses its body to make itself look smaller, which sharply raises its evasiveness.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Check if evasion can be raised further
        if (user.EvasionStage >= 6)
        {
            await UserSession.SendMessageAsync($"Your {user.Name}'s evasiveness won't go any higher!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name}'s evasiveness won't go any higher!");
            return;
        }
        
        for (int i = 0; i < 2; i++)
        {
            if (user.SpeedStage >= 6) {break;}
            user.Speed = (float)(user.MaxSpeed * SkillHelper.CalculateStage(user.SpeedStage));
            user.SpeedStage += 1;
        }
        
        await UserSession.SendMessageAsync($"Your {user.Name} used Minimize! Its evasiveness sharply rose!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Minimize! Its evasiveness sharply rose!");
    }
}