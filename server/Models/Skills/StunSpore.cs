using Server;
using PokemonPocket;

namespace Models;

public class StunSpore : Skill
{
    private StunSpore() { } // For EF Core
    public StunSpore(string PokemonId) : base("Stun Spore", "Grass", 0, 0.75, 30, 1, 0, 0, "The user scatters a cloud of paralyzing powder. It may leave the target with paralysis.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Stun Spore") == false)
            return;

        // Check if target already has a status condition
        if (target.Paralyzed)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Stun Spore, but {target.Name} is already poisoned!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Stun Spore, but your {target.Name} is already poisoned!");
            return;
        }

        // Check if protected by substitute
        if (target.Substitude)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Stun Spore, but {target.Name}'s substitute blocked it!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Stun Spore, but your {target.Name}'s substitute blocked it!");
            return;
        }
        
        // Apply paralysis
        target.Paralyzed = true;
        if (!target.ParalyzeSpeed)
        {
            target.ParalyzeSpeed = true;
            target.Speed *= 0.5f;
        }
        
        await UserSession.SendMessageAsync($"{TargetSession.Username}'s {target.Name} was paralyzed and may be unable to move!");
        await TargetSession.SendMessageAsync($"Your {target.Name} was paralyzed and may be unable to move!");
    }
}