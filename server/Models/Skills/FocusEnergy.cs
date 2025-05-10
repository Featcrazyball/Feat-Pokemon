using Server;
using PokemonPocket;

namespace Models;

public class FocusEnergy : Skill
{
    private FocusEnergy() { } // For EF Core
    public FocusEnergy(string PokemonId) : base("Focus Energy", "Normal", 0, -1, 30, 1, 0, 0, "The user takes a deep breath and focuses to raise its critical-hit ratio.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Raise critical hit ratio
        float oldCritRate = user.CritRate;
        user.CritRate *= 4; // Quadruple crit rate
        if (user.CritRate > 0.996f) user.CritRate = 0.996f; 
        
        await UserSession.SendMessageAsync($"Your {user.Name} used Focus Energy and is getting pumped! (Crit rate: {oldCritRate:F3} → {user.CritRate:F3})");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Focus Energy and is getting pumped! (Crit rate: {oldCritRate:F3} → {user.CritRate:F3})");
    }
}