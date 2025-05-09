using Server;
using PokemonPocket;

namespace Models;

public class Psybeam : Skill
{
    private Psybeam() { } // For EF Core
    public Psybeam(string PokemonId) : base("Psybeam", "Psychic", 65, 1, 20, 1, 0, 0, "The target is attacked with a peculiar ray. It may also leave the target confused.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Psybeam") == false)
            return;

        // Damage calculation
        float damage = await SkillHelper.FeatCalculateSpecialDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Psychic", target.Type?.Split('/') ?? Array.Empty<string>()),  
            UserSession, 
            TargetSession
        );
        
        int turns;
        double hitChance = Random.Shared.NextDouble();
        
        if (hitChance < 0.375) turns = 2;
        else if (hitChance < 0.75) turns = 3;
        else if (hitChance < 0.875) turns = 4;
        else turns = 5;
        
        // Substitute handling
        if (target.Substitude)
        {
            if (target.SubstituteHealth <= damage)
            {
                target.Substitude = false;
                target.SubstituteHealth = 0;
                
                // Check for confusion (10% chance in Gen 1)
                if (!target.Confused && Random.Shared.NextDouble() <= 0.1)
                {
                    target.Confused = true;
                    target.ConfusionTurns = turns; 
                    
                    await UserSession.SendMessageAsync($"Your {user.Name} used Psybeam and broke {target.Name}'s Substitute and confused it!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Psybeam and broke your {target.Name}'s Substitute and confused it!");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Psybeam and broke {target.Name}'s Substitute!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Psybeam and broke your {target.Name}'s Substitute!");
                }
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Psybeam on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Psybeam on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            // Check for confusion (10% chance in Gen 1)
            if (!target.Confused && Random.Shared.NextDouble() <= 0.1)
            {
                target.Confused = true;
                target.ConfusionTurns = turns; 
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Psybeam on {target.Name}, dealing {damage:F1} damage and confusing it!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Psybeam on your {target.Name}, dealing {damage:F1} damage and confusing it!");
            }
            else
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Psybeam on {target.Name}, dealing {damage:F1} damage!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Psybeam on your {target.Name}, dealing {damage:F1} damage!");
            }
        }
    }
}