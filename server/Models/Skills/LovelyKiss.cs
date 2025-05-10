using Server;
using PokemonPocket;

namespace Models;

public class LovelyKiss : Skill
{
    private LovelyKiss() { } // For EF Core
    public LovelyKiss(string PokemonId) : base("Lovely Kiss", "Normal", 0, 0.75, 10, 1, 0, 0, "With a scary face, the user tries to force a kiss on the target. If it succeeds, the target falls asleep.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);
        
        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Lovely Kiss") == false)
            return;

        // Check if target already has a status condition
        if (target.Sleeping)
        {
            await UserSession.SendMessageAsync($"{TargetSession.Username}'s {target.Name} is already Sleeping!");
            await TargetSession.SendMessageAsync($"Your {target.Name} is already Sleeping!");
            return;
        }

        // Check if protected by substitute
        if (target.Substitude)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Lovely Kiss, but {target.Name}'s Substitute protected it!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Lovely Kiss, but your {target.Name}'s Substitute protected it!");
            return;
        }
        
        // Apply sleep
        target.Sleeping = true;
        target.SleepTurns = Random.Shared.Next(1, 4);
        
        await UserSession.SendMessageAsync($"Your {user.Name} used Lovely Kiss! {target.Name} falls asleep for the next {target.SleepTurns} turns!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Lovely Kiss! Your {target.Name} falls asleep for the next {target.SleepTurns} turns!");
    }
}