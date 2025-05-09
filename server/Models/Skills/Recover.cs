using Server;
using PokemonPocket;

namespace Models;

public class Recover : Skill
{
    private Recover() { } // For EF Core
    public Recover(string PokemonId) : base("Recover", "Normal", 0, 1, 10, 0, 0, 0, "Restores the user's HP by up to half of its max HP.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Check if HP is already full
        if (user.Health >= user.MaxHealth)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Recover, but its HP is already full!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Recover, but its HP is already full!");
            return;
        }
        
        // Calculate healing amount (50% of max HP)
        float healAmount = user.MaxHealth / 2;
        float oldHealth = user.Health;
        
        // Apply healing
        user.Health += healAmount;
        if (user.Health > user.MaxHealth)
            user.Health = user.MaxHealth;
            
        float actualHeal = user.Health - oldHealth;
        
        await UserSession.SendMessageAsync($"Your {user.Name} used Recover and restored {actualHeal:F1} HP!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Recover and restored {actualHeal:F1} HP!");
    }
}