using Server;
using PokemonPocket;

namespace Models;

public class SleepPowder : Skill
{
    private SleepPowder() { } // For EF Core
    public SleepPowder(string PokemonId) : base("Sleep Powder", "Grass", 0, 0.75, 15, 1, 0, 0, "The user scatters a big cloud of sleep-inducing dust around the target.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Sleep Powder") == false)
            return;

        // Check if target already has a status condition
        if (target.Sleeping)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Sleep Powder, but {target.Name} is already has a status condition!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Sleep Powder, but your {target.Name} is already has a status condition!");
            return;
        }

        // Check if protected by substitute
        if (target.Substitude)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Sleep Powder, but {target.Name}'s substitute blocked it!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Sleep Powder, but your {target.Name}'s substitute blocked it!");
            return;
        }
        
        // Put target to sleep
        target.Sleeping = true;
        
        // Calculate number of turns (1-3 in Gen 1)
        target.SleepTurns = Random.Shared.Next(1, 4);
        
        await UserSession.SendMessageAsync($"Your {user.Name} used Sleep Powder! {target.Name} fell asleep for {target.SleepTurns}!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Sleep Powder! Your {target.Name} fell asleep for {target.SleepTurns}!");
    }
}