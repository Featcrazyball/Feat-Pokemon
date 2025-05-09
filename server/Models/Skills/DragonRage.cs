using Server;
using PokemonPocket;

namespace Models;

public class DragonRage : Skill
{
    private DragonRage() { } // For EF Core
    public DragonRage(string PokemonId) : base("Dragon Rage", "Dragon", 0, 1, 10, 1, 0, 0, "The user attacks the target with a shock wave of pure rage. It inflicts fixed damage.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, "Dragon Rage") == false)
            return;

        // Fixed damage
        float damage = 40; 

        // Substitute
        if (target.Substitude == true)
        {
            if (target.SubstituteHealth <= damage) 
            {
                target.Substitude = false;
                target.SubstituteHealth = 0;
                await UserSession.SendMessageAsync($"Your {user.Name} used Dragon Rage and broke {target.Name}'s Substitute!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Dragon Rage and broke your {target.Name}'s Substitute!");
            }
            else
            {
                target.SubstituteHealth -= damage;
                await UserSession.SendMessageAsync($"Your {user.Name} used Dragon Rage on {target.Name}'s Substitute, dealing {damage} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Dragon Rage on your {target.Name}'s Substitute, dealing {damage} damage.");
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            await UserSession.SendMessageAsync($"Your {user.Name} used Dragon Rage on {target.Name}, dealing {damage} damage.");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Dragon Rage on your {target.Name}, dealing {damage} damage.");
        }

    }
}