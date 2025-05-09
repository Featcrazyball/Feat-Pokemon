using Server;
using PokemonPocket;

namespace Models;

public class LeechSeed : Skill
{
    private LeechSeed() { } // For EF Core
    public LeechSeed(string PokemonId) : base("Leech Seed", "Grass", 0, 0.9, 10, 1, 0, 0, "A seed is planted on the target. It steals some HP from the target every turn to heal the user.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Leech Seed") == false)
            return;
            
        // Cannot affect Grass-type Pokémon
        if (target.Type != null && target.Type.Contains("Grass"))
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Leech Seed, but it had no effect on {target.Name}!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Leech Seed, but it had no effect on your {target.Name}!");
            return;
        }
        
        // Check if protected by substitute
        if (target.Substitude)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Leech Seed, but {target.Name}'s Substitute protected it!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Leech Seed, but your {target.Name}'s Substitute protected it!");
            return;
        }
        
        // Check if already seeded
        if (target.LeechSeed)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Leech Seed, but {target.Name} is already seeded!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Leech Seed, but your {target.Name} is already seeded!");
            return;
        }
        
        // Apply Leech Seed
        target.LeechSeed = true;
        target.LeechSeedTurns = -1; // Lasts until the Pokémon switches out
        
        await UserSession.SendMessageAsync($"Your {user.Name} used Leech Seed and planted seeds on {target.Name}!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Leech Seed and planted seeds on your {target.Name}!");
    }
}