using Server;
using PokemonPocket;

namespace Models;

public class Harden : Skill
{
    private Harden() { } // For EF Core
    public Harden(string PokemonId) : base("Harden", "Normal", 0, -1, 30, 1, 0, 0, "The user stiffens its body's muscles to raise its Defense stat.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Check if Defense can be raised further
        if (user.DefenseStage >= 6)
        {
            await UserSession.SendMessageAsync($"Your {user.Name}'s Defense won't go any higher!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name}'s Defense won't go any higher!");
            return;
        }
        
        // Raise Defense
        user.DefenseStage += 1;
        user.Defense = user.MaxDefense * (float)SkillHelper.CalculateStage(user.DefenseStage);
        
        await UserSession.SendMessageAsync($"Your {user.Name} used Harden! Its Defense rose!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Harden! Its Defense rose!");
    }
}