using Server;
using PokemonPocket;

namespace Models;

public class TailWhip : Skill
{
    private TailWhip() { } // For EF Core
    public TailWhip(string PokemonId) : base("Tail Whip", "Normal", 0, 1, 30, 1, 0, 0, "The user wags its tail cutely, making opposing Pokémon less wary and lowering their Defense stat.", PokemonId)    
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
            await UserSession.SendMessageAsync($"Your {user.Name} used Tail Whip, but {target.Name}'s substitute blocked it!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Tail Whip, but your {target.Name}'s substitute blocked it!");
            return;
        }

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Tail Whip") == false)
            return;
        
        // Check if Defense can be lowered further
        if (target.DefenseStage <= -6) {
            await UserSession.SendMessageAsync($"Your {user.Name} used Tail Whip, but {target.Name}'s Defense won't go any lower!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Tail Whip, but your {target.Name}'s Defense won't go any lower!");
            return;
        }
        
        if (target.Mist) {
            await UserSession.SendMessageAsync($"Your {user.Name} used Tail Whip, but it failed due to Mist!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Tail Whip, but it failed due to Mist!");
            return;
        }

        // Lower Defense by 1 stage
        target.DefenseStage -= 1;
        target.Defense = (float)(target.MaxDefense * SkillHelper.CalculateStage(target.DefenseStage));

        await UserSession.SendMessageAsync($"Your {user.Name} used Tail Whip, lowering {target.Name}'s Defense by 1 Stage to {target.Defense:F1}!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Tail Whip, lowering your {target.Name}'s Defense by 1 Stage to {target.Defense:F1}!");
    }
}