using Server;
using PokemonPocket;

namespace Models;

public class Glare : Skill
{
    private Glare() { } // For EF Core
    public Glare(string PokemonId) : base("Glare", "Normal", 0, 0.75, 30, 1, 0, 0, "The user intimidates the target with the pattern on its body to cause paralysis.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Glare") == false)
            return;
        
        // Check for type immunity (Ground types are immune to paralysis in later gens, but in Gen 1 they weren't)
        if (SkillHelper.CheckPhysical(target.Type ?? "unknown"))
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Glare, but it had no effect on {target.Name}!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Glare, but it had no effect on your {target.Name}!");
            return;
        }

        // Check if protected by substitute
        if (target.Substitude)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Glare, but {target.Name}'s Substitute protected it!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Glare, but your {target.Name}'s Substitute protected it!");
            return;
        }
        
        // Apply paralysis
        target.Paralyzed = true;
        if (!target.ParalyzeSpeed)
        {
            target.ParalyzeSpeed = true;
            target.Speed *= 0.5f;
        }
        
        await UserSession.SendMessageAsync($"Your {user.Name} used Glare! {target.Name} was paralyzed and may be unable to move!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Glare! Your {target.Name} was paralyzed and may be unable to move!");

    }
}