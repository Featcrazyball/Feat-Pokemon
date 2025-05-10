using Server;
using PokemonPocket;

namespace Models;

public class SoftBoiled : Skill
{
    private SoftBoiled() { } // For EF Core
    public SoftBoiled(string PokemonId) : base("Soft-Boiled", "Normal", 0, 1, 10, 1, 0, 0, "The user restores its own HP by up to half of its maximum HP.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Check if health is already full
        if (user.Health >= user.MaxHealth) {
            await UserSession.SendMessageAsync($"Your {user.Name} used Soft-Boiled, but its HP is already full!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Soft-Boiled, but its HP is already full!");
            return;
        }
        
        // Healing logic
        float recoveredHealth = user.MaxHealth * 0.5f;
        float difference = user.MaxHealth - user.Health;
        user.Health += recoveredHealth;

        if (user.Health > user.MaxHealth) {
            recoveredHealth = difference;
            user.Health = user.MaxHealth;
        }

        await UserSession.SendMessageAsync($"Your {user.Name} used Soft-Boiled and restored {recoveredHealth} of its health!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Soft-Boiled and restored {recoveredHealth} of its health!");
    }
}