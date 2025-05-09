using Server;
using PokemonPocket;

namespace Models;

public class Hypnosis : Skill
{
    private Hypnosis() { } // For EF Core
    public Hypnosis(string PokemonId) : base("Hypnosis", "Psychic", 0, 0.6, 20, 1, 0, 0, "The user employs hypnotic suggestion to make the target fall into a deep sleep.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Hypnosis") == false)
            return;

        // Check if target already has a status condition
        if (target.Sleeping)
        {
            await UserSession.SendMessageAsync($"{target.Name} is already sleeping!");
            await TargetSession.SendMessageAsync($"Your {target.Name} is already sleeping!");
            return;
        }

        // Check if protected by substitute
        if (target.Substitude)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Hypnosis, but {target.Name}'s Substitute protected it!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Hypnosis, but your {target.Name}'s Substitute protected it!");
            return;
        }
        
        // Apply sleep
        target.Sleeping = true;
        target.SleepTurns = Random.Shared.Next(1, 4);
        
        await UserSession.SendMessageAsync($"Your {user.Name} used Hypnosis! {target.Name} fell asleep!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Hypnosis! Your {target.Name} fell asleep!");
    }
}