using Server;
using PokemonPocket;

namespace Models;

public class SuperFang : Skill
{
    private SuperFang() { } // For EF Core
    public SuperFang(string PokemonId) : base("Super Fang", "Normal", 0, 0.9, 10, 1, 0, 0, "The user chomps hard on the target with its sharp front fangs. This cuts the target's HP in half.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Super Fang") == false)
            return;

        // Damage is half of current HP
        float damage = target.Health / 2;
        
        // Substitute handling
        if (target.Substitude)
        {
            if (target.SubstituteHealth <= damage)
            {
                target.Substitude = false;
                target.SubstituteHealth = 0;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Super Fang and broke {target.Name}'s Substitute!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Super Fang and broke your {target.Name}'s Substitute!");
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Super Fang on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Super Fang on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            await UserSession.SendMessageAsync($"Your {user.Name} used Super Fang on {target.Name}, cutting its HP in half!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Super Fang on your {target.Name}, cutting its HP in half!");
        }
    }
}