using Server;
using PokemonPocket;

namespace Models;

public class Counter : Skill
{
    private Counter() { } // For EF Core
    public Counter(string PokemonId) : base("Counter", "Fighting", 0, 1, 20, 1, 0, -5, "A retaliation move that counters any physical attack, inflicting double the damage taken.", PokemonId)    
    {
        this.PokemonId = PokemonId;
        Priority = -5;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        if (target.Lastmove == null) 
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Counter, but it failed because the target has not attacked yet.");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Counter, but it failed because the target has not attacked yet.");
            return;
        }

        if (!SkillHelper.CheckPhysical(target.Lastmove.Name ?? "Unknown"))
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Counter, but it failed because {UserSession.Username}'s {user.Name}'s last move was not a physical attack.");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Counter, but it failed because your {target.Name} last move was not a physical attack.");
            return;
        }

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Counter") == false) {return;}

        float damage = await SkillHelper.FeatCalculateDamage(
            target.Lastmove!.BasePower * 2, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Fighting", target.Type?.Split('/') ?? Array.Empty<string>()),  
            UserSession, 
            TargetSession
        );

        // Substitude
        if (target.Substitude)
        {
            if (target.SubstituteHealth <= damage)
            {
                target.Substitude = false;
                target.SubstituteHealth = 0;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Counter and broke {target.Name}'s Substitute!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Counter and broke your {target.Name}'s Substitute!");
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Counter on {target.Name}'s Substitute, dealing {damage:F1} damage!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Counter on your {target.Name}'s Substitute, dealing {damage:F1} damage!");
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            await UserSession.SendMessageAsync($"Your {user.Name} used Counter on {target.Name}, dealing {damage:F1} damage!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Counter on your {target.Name}, dealing {damage:F1} damage!");
        }

    }
}

