using Server;
using PokemonPocket;

namespace Models;

public class Wrap : Skill
{
    private Wrap() { } // For EF Core
    public Wrap(string PokemonId) : base("Wrap", "Normal", 15, 0.9, 20, 1, 0, 0, "A long body, vines, or the like are used to wrap and squeeze the target for four to five turns.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);
        
        // Check if target is already being bound
        if (target.BindActive) {
            await UserSession.SendMessageAsync($"Your {user.Name} used Wrap, but {target.Name} is already trapped!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Wrap, but your {target.Name} is already trapped!");
            return;
        }

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Wrap") == false)
            return;

        // Calculate number of turns (2-5)
        int turns;
        float chance = Random.Shared.Next(0, 100);
        if (chance > 87.5) turns = 5;
        else if (chance > 75) turns = 4;
        else if (chance > 37.5) turns = 3;
        else turns = 2;

        // Damage calculation
        float damage = await SkillHelper.FeatCalculateDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Normal", target.Type?.Split('/') ?? Array.Empty<string>()),  
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
                
                // Apply bind effect
                target.BindDamage = damage;
                target.BindTurns = turns;
                target.BindActive = true;

                await UserSession.SendMessageAsync($"Your {user.Name} used Wrap and broke {target.Name}'s Substitute and wrapped around {target.Name}!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Wrap and broke your {target.Name}'s Substitute and wrapped around your {target.Name}!");
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Wrap on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Wrap on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            // Apply bind effect
            target.BindDamage = damage;
            target.BindTurns = turns;
            target.BindActive = true;

            await UserSession.SendMessageAsync($"Your {user.Name} used Wrap on {target.Name}, dealing {damage:F1} damage and wrapping around it!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Wrap on your {target.Name}, dealing {damage:F1} damage and wrapping around it!");
        }
    }
}