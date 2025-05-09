using Server;
using PokemonPocket;

namespace Models;

public class Leer : Skill
{
    private Leer() { } // For EF Core
    public Leer(string PokemonId) : base("Leer", "Normal", 0, 1, 30, 1, 0, 0, "The user gives opposing Pokémon an intimidating leer that lowers the Defense stat.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Leer") == false)
            return;
        
        // Check if protected by substitute
        if (target.Substitude)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Leer, but {target.Name}'s Substitute protected it!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Leer, but your {target.Name}'s Substitute protected it!");
            return;
        }
        
        // Check if Defense can be lowered further
        if (target.DefenseStage <= -6)
        {
            await UserSession.SendMessageAsync($"{target.Name}'s Defense won't go any lower!");
            await TargetSession.SendMessageAsync($"Your {target.Name}'s Defense won't go any lower!");
            return;
        }
        
        // Lower Defense
        if (!target.Mist)
        {
            target.DefenseStage -= 1;
            target.Defense = target.MaxDefense * (float)SkillHelper.CalculateStage(target.DefenseStage);
        }
        
        if (target.Mist)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Leer on {target.Name}, but its Defense was not lowered due to Mist!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Leer on your {target.Name}, but its Defense was not lowered due to Mist!");
        }
        else
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Leer! {target.Name}'s Defense fell by 1 Stage!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Leer! Your {target.Name}'s Defense fell by 1 Stage!");
        }
    }
}