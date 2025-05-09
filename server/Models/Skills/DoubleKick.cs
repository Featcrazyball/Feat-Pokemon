using Server;
using PokemonPocket;

namespace Models;

public class DoubleKick : Skill
{
    private DoubleKick() { } // For EF Core
    public DoubleKick(string PokemonId) : base("Double Kick", "Fighting", 30, 2, 10, 1, 0, 0, "The user strikes the target twice in quick succession.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Calculate number of hits (2-5)
        int hits = 2;
        int missed = 0;
        bool broken = false;
        float damageToPokemon = 0;
        float damageToSubstitute = 0;

        float damage =0;
            
        // Accuracy check
        for (int i = 0; i < hits; i++)
        {
            if (Random.Shared.NextDouble() < (Accuracy * (SkillHelper.CalculateStage(user.AccuracyStage) / SkillHelper.CalculateStage(target.EvasionStage))))
            {  
                damage = ((2 * user.Level + 2) * damage * user.Attack / target.Defense / 50 + 2) * SkillHelper.QuietGetEffectiveness("Fighting", target.Type?.Split('/') ?? Array.Empty<string>()) ;
                
                // Substitute handling
                if (target.Substitude)
                {
                    if (target.SubstituteHealth <= damage)
                    {
                        target.Substitude = false;
                        target.SubstituteHealth = 0;
                        broken = true;
                    }
                    else
                    {
                        target.SubstituteHealth -= damage;
                        
                        damageToSubstitute += damage;
                        if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
                    }
                }
                else
                {
                    target.Health -= damage;
                    await SkillHelper.ProcessRage(target, TargetSession, UserSession);
                    damageToPokemon += damage;
                }
            } else {
                missed++;
            }
        }
        
        if (missed == hits) 
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Double Kick, but missed all its hits!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Double Kick, but missed all its hits!");
            return;
        }

        await UserSession.SendMessageAsync($"Your {user.Name} used Double Kick, hitting {hits - missed} times and missing {missed} times!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Double Kick, hitting {hits - missed} times and missing {missed} times!");

        if (broken) 
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Double Kick and broke {target.Name}'s Substitute!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Double Kick and broke your {target.Name}'s Substitute!");
        }

        if (target.Substitude)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Double Kick on {target.Name}'s Substitute, dealing {damageToSubstitute:F1} damage.");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Double Kick on your {target.Name}'s Substitute, dealing {damageToSubstitute:F1} damage.");
        }

        if (!target.Substitude && damageToPokemon > 0)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Double Kick on {target.Name}, dealing {damageToPokemon:F1} damage!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Double Kick on your {target.Name}, dealing {damageToPokemon:F1} damage!");
        }
    }
}