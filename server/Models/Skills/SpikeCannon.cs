using Server;
using PokemonPocket;

namespace Models;

public class SpikeCannon : Skill
{
    private SpikeCannon() { } // For EF Core
    public SpikeCannon(string PokemonId) : base("Spike Cannon", "Normal", 20, 1, 15, 1, 0, 0, "Sharp spikes are shot at the target in rapid succession. They hit two to five times in a row.", PokemonId)    
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

        double hitChance = Random.Shared.NextDouble();
        
        if (hitChance < 0.375)
            hits = 2;
        else if (hitChance < 0.75)
            hits = 3;
        else if (hitChance < 0.875)
            hits = 4;
        else
            hits = 5;
            
        // Accuracy check and damage calculation for each hit
        for (int i = 0; i < hits; i++)
        {
            if (Random.Shared.NextDouble() < (Accuracy * (SkillHelper.CalculateStage(user.AccuracyStage) / SkillHelper.CalculateStage(target.EvasionStage))))
            {  
                float damage = await SkillHelper.FeatCalculateDamage(
                    BasePower, 
                    user, 
                    target, 
                    SkillHelper.QuietGetEffectiveness("Normal", target.Type?.Split('/') ?? Array.Empty<string>()),
                    UserSession, 
                    TargetSession
                );
                
                // Substitute handling
                if (target.Substitude)
                {
                    if (target.SubstituteHealth <= damage)
                    {
                        target.Substitude = false;
                        target.SubstituteHealth = 0;
                        broken = true;
                        break;
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
                    damageToPokemon += damage;
                    await SkillHelper.ProcessRage(target, TargetSession, UserSession);
                }
            }
            else
            {
                missed++;
            }
        }
        
        await UserSession.SendMessageAsync($"Your {user.Name} used Spike Cannon, hitting {hits - missed} times and missing {missed} times!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Spike Cannon, hitting {hits - missed} times and missing {missed} times!");

        if (broken) 
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Spike Cannon and broke {target.Name}'s Substitute!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Spike Cannon and broke your {target.Name}'s Substitute!");
        }
        else if (target.Substitude && damageToSubstitute > 0)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Spike Cannon on {target.Name}'s Substitute, dealing {damageToSubstitute:F1} damage.");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Spike Cannon on your {target.Name}'s Substitute, dealing {damageToSubstitute:F1} damage.");
        }
        else if (!target.Substitude && damageToPokemon > 0)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Spike Cannon on {target.Name}, dealing {damageToPokemon:F1} damage!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Spike Cannon on your {target.Name}, dealing {damageToPokemon:F1} damage!");
        }
    }
}