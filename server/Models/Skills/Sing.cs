using Server;
using PokemonPocket;

namespace Models;

public class Sing : Skill
{
    private Sing() { } // For EF Core
    public Sing(string PokemonId) : base("Sing", "Normal", 0, 0.55, 15, 1, 0, 0, "A soothing lullaby is sung in a calming voice that puts the target into a deep slumber.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Check if substitute is present
        if (target.Substitude)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Sing, but {target.Name}'s substitute blocked it!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Sing, but your {target.Name}'s substitute blocked it!");
            return;
        }

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Sing") == false)
            return;

        // Check if target already has a status effect
        if (target.Sleeping)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Sing, but it failed because {target.Name} is already sleeping!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Sing, but it failed because your {target.Name} is already sleeping!");
            return;
        }

        // Put target to sleep
        target.Sleeping = true;
        
        target.SleepTurns = Random.Shared.Next(1, 4);
        
        await UserSession.SendMessageAsync($"Your {user.Name} used Sing! {target.Name} fell asleep for {target.SleepTurns}!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Sing! Your {target.Name} fell asleep for {target.SleepTurns}!");
    }
}