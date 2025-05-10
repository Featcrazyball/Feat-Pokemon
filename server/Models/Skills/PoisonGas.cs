using Server;
using PokemonPocket;

namespace Models;

public class PoisonGas : Skill
{
    private PoisonGas() { } // For EF Core
    public PoisonGas(string PokemonId) : base("Poison Gas", "Poison", 0, 0.9, 40, 1, 0, 0, "A cloud of poison gas is released in the target's face. It may poison the target.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Poison Gas") == false)
            return;

        // Check if target already has a status condition
        if (target.Poisoned && !target.BadlyPoisoned)
        {
            await UserSession.SendMessageAsync($"{target.Name} already has a status condition!");
            await TargetSession.SendMessageAsync($"Your {target.Name} already has a status condition!");
            return;
        }

        // Check if protected by substitute
        if (target.Substitude)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Poison Gas, but {target.Name}'s Substitute protected it!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Poison Gas, but your {target.Name}'s Substitute protected it!");
            return;
        }

        // Check for type immunity (Poison types can't be poisoned)
        if (target.Type != null && target.Type.Contains("Poison"))
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Poison Gas, but it had no effect on {target.Name}!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Poison Gas, but it had no effect on your {target.Name}!");
            return;
        }
        
        // Apply poison
        target.Poisoned = true;
        
        await UserSession.SendMessageAsync($"Your {user.Name} used Poison Gas! {target.Name} was poisoned!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Poison Gas! Your {target.Name} was poisoned!");
    }
}