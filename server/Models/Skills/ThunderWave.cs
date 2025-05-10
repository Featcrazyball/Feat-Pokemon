using Server;
using PokemonPocket;

namespace Models;

public class ThunderWave : Skill
{
    private ThunderWave() { } // For EF Core
    public ThunderWave(string PokemonId) : base("Thunder Wave", "Electric", 0, 1, 20, 1, 0, 0, "The user launches a weak jolt of electricity that paralyzes the target.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Thunder Wave") == false)
            return;

        // Check if target already has a status condition
        if (target.Paralyzed)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Thunder Wave, but {target.Name} is already paralyzed!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Thunder Wave, but your {target.Name} is already paralyzed!");
            return;
        }

        // Check if protected by substitute
        if (target.Substitude)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Thunder Wave, but {target.Name}'s substitute blocked it!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Thunder Wave, but your {target.Name}'s substitute blocked it!");
            return;
        }
        
        // Apply paralysis
        target.Paralyzed = true;
        if (!target.ParalyzeSpeed)
        {
            target.ParalyzeSpeed = true;
            target.Speed *= 0.5f;
        }
        
        await UserSession.SendMessageAsync($"Your {user.Name} used Thunder Wave! {target.Name} was paralyzed and may be unable to move!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Thunder Wave! Your {target.Name} was paralyzed and may be unable to move!");
    }
}