using Server;
using PokemonPocket;

namespace Models;

public class SandAttack : Skill
{
    private SandAttack() { } // For EF Core
    public SandAttack(string PokemonId) : base("Sand Attack", "Ground", 0, 1, 15, 1, 0, 0, "Sand is hurled in the target's face, reducing its accuracy.", PokemonId)    
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
            await UserSession.SendMessageAsync($"Your {user.Name} used Sand Attack, but {target.Name}'s substitute blocked it!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Sand Attack, but your {target.Name}'s substitute blocked it!");
            return;
        }

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Sand Attack") == false)
            return;

        if (target.AccuracyStage <= -6) {
            await UserSession.SendMessageAsync($"Your {user.Name} used Sand Attack, but {target.Name}'s Accuracy won't go any lower!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Sand Attack, but your {target.Name}'s Accuracy won't go any lower!");
            return;
        }
        
        target.AccuracyStage -= 1;

        await UserSession.SendMessageAsync($"Your {user.Name} used Sand Attack, lowering {target.Name}'s Accuracy!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Sand Attack, lowering your {target.Name}'s Accuracy!");
    }
}