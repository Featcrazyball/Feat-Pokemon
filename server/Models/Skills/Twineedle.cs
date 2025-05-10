using Server;
using PokemonPocket;

namespace Models;

public class Twineedle : Skill
{
    private Twineedle() { } // For EF Core
    public Twineedle(string PokemonId) : base("Twineedle", "Bug", 25, 1, 20, 1, 0, 0, "The user damages the target twice in succession by jabbing it with two spikes. It may also poison the target.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Twineedle always hits twice if the first hit lands (calculate hits)
        int hits = 2;
        int missed = 0;
        bool broken = false;
        float damageToPokemon = 0;
        float damageToSubstitute = 0;
        bool causedPoison = false;

        // Accuracy check and damage calculation for each hit
        for (int i = 0; i < hits; i++)
        {
            // First hit accuracy check
            if (i == 0 && await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Twineedle") == false)
            {
                missed = 2; 
                break;
            }
            // Second hit guaranteed if first hit lands
            
            // Calculate damage for this hit
            float damage = await SkillHelper.FeatCalculateDamage(
                BasePower, 
                user, 
                target, 
                await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Bug", target.Type?.Split('/') ?? Array.Empty<string>()),  
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
                    
                    // 20% chance to poison on last hit
                    if (i == hits - 1 && !target.Poisoned && !target.BadlyPoisoned && 
                        Random.Shared.NextDouble() <= 0.2 && (target.Type == null || !target.Type.Contains("Poison")))
                    {
                        target.Poisoned = true;
                        causedPoison = true;
                    }

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
                
                // 20% chance to poison on last hit
                if (i == hits - 1 && !target.Poisoned && !target.BadlyPoisoned && 
                    Random.Shared.NextDouble() <= 0.2 && (target.Type == null || !target.Type.Contains("Poison")))
                {
                    target.Poisoned = true;
                    causedPoison = true;
                }
            }
        }
        
        // Message based on outcome
        if (missed == hits) 
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Twineedle, but missed!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Twineedle, but missed!");
            return;
        }

        await UserSession.SendMessageAsync($"Your {user.Name} used Twineedle, hitting {hits - missed} times!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Twineedle, hitting {hits - missed} times!");

        if (broken) 
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Twineedle and broke {target.Name}'s Substitute!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Twineedle and broke your {target.Name}'s Substitute!");
        }
        else if (target.Substitude)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Twineedle on {target.Name}'s Substitute, dealing {damageToSubstitute:F1} damage.");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Twineedle on your {target.Name}'s Substitute, dealing {damageToSubstitute:F1} damage.");
        }
        else if (!target.Substitude && damageToPokemon > 0)
        {
            if (causedPoison)
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Twineedle on {target.Name}, dealing {damageToPokemon:F1} damage! {target.Name} was poisoned!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Twineedle on your {target.Name}, dealing {damageToPokemon:F1} damage! Your {target.Name} was poisoned!");
            }
            else
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Twineedle on {target.Name}, dealing {damageToPokemon:F1} damage!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Twineedle on your {target.Name}, dealing {damageToPokemon:F1} damage!");
            }
        }
    }
}