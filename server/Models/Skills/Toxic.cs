using Server;
using PokemonPocket;

namespace Models;

public class Toxic : Skill
{
    private Toxic() { } // For EF Core
    public Toxic(string PokemonId) : base("Toxic", "Poison", 0, 0.9, 10, 1, 0, 0, "A move that leaves the target badly poisoned. Its poison damage worsens every turn.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Toxic") == false)
            return;
        
        // Check if immune due to typing
        if (target.Type != null && target.Type.Contains("Poison"))
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Toxic, but {target.Name} is immune!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Toxic, but your {target.Name} is immune!");
            return;
        }

        // Check if target already has a status condition
        if (target.BadlyPoisoned)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Toxic, but {target.Name} is already badly poisoned!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Toxic, but your {target.Name} is already badly poisoned!");
            return;
        }

        if (target.Poisoned)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Toxic, but {target.Name} is already poisoned!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Toxic, but your {target.Name} is already poisoned!");
            return;
        }

        // Check if protected by substitute
        if (target.Substitude)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Toxic, but {target.Name}'s substitute blocked it!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Toxic, but your {target.Name}'s substitute blocked it!");
            return;
        }

        // Apply badly poisoned status
        target.BadlyPoisoned = true;
        target.BadlyPoisonedTurns = 1;
        
        await UserSession.SendMessageAsync($"Your {user.Name} used Toxic! {target.Name} was badly poisoned!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Toxic! Your {target.Name} was badly poisoned!");
    }
}