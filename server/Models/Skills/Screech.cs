using Server;
using PokemonPocket;

namespace Models;

public class Screech : Skill
{
    private Screech() { } // For EF Core
    public Screech(string PokemonId) : base("Screech", "Normal", 0, 0.85, 40, 1, 0, 0, "An earsplitting screech harshly lowers the target's Defense stat.", PokemonId)    
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
            await UserSession.SendMessageAsync($"Your {user.Name} used Screech, but {target.Name}'s substitute blocked it!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Screech, but your {target.Name}'s substitute blocked it!");
            return;
        }

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Screech") == false)
            return;

        if (target.Mist) {
            await UserSession.SendMessageAsync($"Your {user.Name} used Screech, but it failed due to Mist!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Screech, but it failed due to Mist!");
            return;
        }

        if (target.DefenseStage <= -6) {
            await UserSession.SendMessageAsync($"Your {user.Name} used Screech, but {target.Name}'s Defense won't go any lower!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Screech, but your {target.Name}'s Defense won't go any lower!");
            return;
        }
        
        bool max=false;
        // Lower Defense by 2 stages
        for (int i=0; i<2; i++)
        {
            if (target.DefenseStage <= -6) {
                max=true;
                break;
            }
            target.DefenseStage -= 2;
            if (target.DefenseStage < -6) target.DefenseStage = -6;
        }

        target.Defense = (float)(target.MaxDefense * SkillHelper.CalculateStage(target.DefenseStage));

        if (max)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Screech, harshly lowering {target.Name}'s Defense by 1 Stage!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Screech, harshly lowering your {target.Name}'s Defense by 1 Stage!");
        }
        else
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Screech, harshly lowering {target.Name}'s Defense by 2 Stages!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Screech, harshly lowering your {target.Name}'s Defense by 2 Stages!");
        }
    }
}