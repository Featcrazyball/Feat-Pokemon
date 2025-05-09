using Server;
using PokemonPocket;

namespace Models;

public class DoubleSlap : Skill
{
    private DoubleSlap() { } // For EF Core
    public DoubleSlap(string PokemonId) : base("Double Slap", "Normal", 15, 1, 10, 1, 0, 0, "The user slaps the target with two to five hard slaps in a row.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Calculate number of hits (2-5)
        int hits;
        int missed = 0;
        bool broken = false;
        float damageToPokemon = 0;
        float damageToSubstitute = 0;

        float damage =0;
        double hitChance = Random.Shared.NextDouble();
        
        if (hitChance < 0.375)
            hits = 2;
        else if (hitChance < 0.75)
            hits = 3;
        else if (hitChance < 0.875)
            hits = 4;
        else
            hits = 5;
            
        // Accuracy check
        for (int i = 0; i < hits; i++)
        {
            if (Random.Shared.NextDouble() < (Accuracy * (SkillHelper.CalculateStage(user.AccuracyStage) / SkillHelper.CalculateStage(target.EvasionStage))))
            {  
                damage = ((2 * user.Level + 2) * damage * user.Attack / target.Defense / 50 + 2) * SkillHelper.QuietGetEffectiveness("Normal", target.Type?.Split('/') ?? Array.Empty<string>()) ;
                
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
            await UserSession.SendMessageAsync($"Your {user.Name} used Double Slap, but missed all its hits!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Double Slap, but missed all its hits!");
            return;
        }
        
        await UserSession.SendMessageAsync($"Your {user.Name} used Double Slap, hitting {hits - missed} times and missing {missed} times!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Double Slap, hitting {hits - missed} times and missing {missed} times!");

        if (broken) 
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Double Slap and broke {target.Name}'s Substitute!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Double Slap and broke your {target.Name}'s Substitute!");
        }

        if (target.Substitude)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Double Slap on {target.Name}'s Substitute, dealing {damageToSubstitute:F1} damage.");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Double Slap on your {target.Name}'s Substitute, dealing {damageToSubstitute:F1} damage.");
        }

        if (!target.Substitude && damageToPokemon > 0)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Double Slap on {target.Name}, dealing {damageToPokemon:F1} damage!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Double Slap on your {target.Name}, dealing {damageToPokemon:F1} damage!");
        }

    }
}