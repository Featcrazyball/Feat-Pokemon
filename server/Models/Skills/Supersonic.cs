using Server;
using PokemonPocket;

namespace Models;

public class Supersonic : Skill
{
    private Supersonic() { } // For EF Core
    public Supersonic(string PokemonId) : base("Supersonic", "Normal", 0, 0.55, 20, 1, 0, 0, "The user generates odd sound waves from its body that confuse the target.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Check if already confused
        if (target.Confused)
        {
            await UserSession.SendMessageAsync($"{target.Name} is already confused!");
            await TargetSession.SendMessageAsync($"Your {target.Name} is already confused!");
            return;
        }

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Supersonic") == false)
            return;

        // Check if protected by substitute
        if (target.Substitude)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Supersonic, but {target.Name}'s substitute blocked it!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Supersonic, but your {target.Name}'s substitute blocked it!");
            return;
        }
        
        // Apply confusion
        target.Confused = true;
        
        double hitChance = Random.Shared.NextDouble();
        int turns;
        if (hitChance < 0.375) turns = 2;
        else if (hitChance < 0.75) turns = 3;
        else if (hitChance < 0.875) turns = 4;
        else turns = 5;

        target.ConfusionTurns = turns; 
        
        await UserSession.SendMessageAsync($"Your {user.Name} used Supersonic! {target.Name} became confused!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Supersonic! Your {target.Name} became confused!");
    }
}