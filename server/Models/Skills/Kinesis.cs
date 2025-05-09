using Server;
using PokemonPocket;

namespace Models;

public class Kinesis : Skill
{
    private Kinesis() { } // For EF Core
    public Kinesis(string PokemonId) : base("Kinesis", "Psychic", 0, 0.8, 15, 1, 0, 0, "The user distracts the target by bending a spoon. It lowers the target's accuracy.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Kinesis") == false)
            return;

        // Check if protected by substitute
        if (target.Substitude)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Kinesis, but {target.Name}'s Substitute protected it!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Kinesis, but your {target.Name}'s Substitute protected it!");
            return;
        }
        
        // Check if accuracy can be lowered further
        if (target.AccuracyStage <= -6)
        {
            await UserSession.SendMessageAsync($"{target.Name}'s accuracy won't go any lower!");
            await TargetSession.SendMessageAsync($"Your {target.Name}'s accuracy won't go any lower!");
            return;
        }
        
        // Lower accuracy
        target.AccuracyStage -= 1;
        
        await UserSession.SendMessageAsync($"Your {user.Name} used Kinesis! {target.Name}'s accuracy fell!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Kinesis! Your {target.Name}'s accuracy fell!");
    }
}