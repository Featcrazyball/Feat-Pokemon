using Server;
using PokemonPocket;

namespace Models;

public class DreamEater : Skill
{
    private DreamEater() { } // For EF Core
    public DreamEater(string PokemonId) : base("Dream Eater", "Psychic", 100, 1, 15, 1, 0, 0, "The user attacks the target's dreams. The user recovers half the damage inflicted on the target.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, "Dream Eater") == false)
            return;

        // Check if target is asleep
        if (!target.Sleeping) {
            await UserSession.SendMessageAsync($"Your {user.Name} used Dream Eater on {target.Name}, but it is not asleep!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Dream Eater on your {target.Name}, but it is not asleep!");
            return;
        }
        if (target.Substitude == true) {
            await UserSession.SendMessageAsync($"Your {user.Name} used Dream Eater on {target.Name}'s Substitute, but it is not asleep!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Dream Eater on your {target.Name}'s Substitute, but it is not asleep!");
            return;
        }

        // Calculate damage
        float damage = await SkillHelper.FeatCalculateDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Psychic", target.Type?.Split('/') ?? Array.Empty<string>()),  
            UserSession, 
            TargetSession
        );
        if (damage < 0) damage = 0; 

        // Apply damage to target
        float recovery = damage > target.Health ?  target.Health/2 : damage / 2;
        target.Health -= damage;
        await SkillHelper.ProcessRage(target, TargetSession, UserSession);

        if (target.Health < 0) {target.Health = 0;}
        await UserSession.SendMessageAsync($"Your {user.Name} used Dream Eater on {target.Name}, dealing {damage} damage.");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Dream Eater on your {target.Name}, dealing {damage} damage.");
    
        // Recover half the damage inflicted on the target
        user.Health += recovery;
        if (user.Health > user.MaxHealth) {user.Health = user.MaxHealth;}
        await UserSession.SendMessageAsync($"Your {user.Name} recovered {recovery} health from Dream Eater.");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} recovered {recovery} health from Dream Eater.");

    }
}