using Server;
using PokemonPocket;

namespace Models;

public class HighJumpKick : Skill
{
    private HighJumpKick() { } // For EF Core
    public HighJumpKick(string PokemonId) : base("High Jump Kick", "Fighting", 130, 0.9, 10, 1, 0, 0, "The user launches a jumping knee strike. If it misses, the user is hurt instead.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check - special handling for crash damage
        if (Random.Shared.NextDouble() > (Accuracy * (SkillHelper.CalculateStage(user.AccuracyStage) / SkillHelper.CalculateStage(target.EvasionStage))))
        {
            // In Gen 1, crash damage is 1 HP, but later gens use half of max HP
            float crashDamage = user.MaxHealth / 2;
            user.Health -= crashDamage;

            if (user.Health < 0) user.Health = 0;
            
            await UserSession.SendMessageAsync($"Your {user.Name} used High Jump Kick, but it missed and crashed, taking {crashDamage:F1} damage!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used High Jump Kick, but it missed and crashed, taking {crashDamage:F1} damage!");
            return;
        }

        // Damage calculation
        float damage = await SkillHelper.FeatCalculateDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Fighting", target.Type?.Split('/') ?? Array.Empty<string>()),  
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
                
                await UserSession.SendMessageAsync($"Your {user.Name} used High Jump Kick and broke {target.Name}'s Substitute!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used High Jump Kick and broke your {target.Name}'s Substitute!");
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used High Jump Kick on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used High Jump Kick on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) {target.SubstituteHealth = 0;}
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            await UserSession.SendMessageAsync($"Your {user.Name} used High Jump Kick on {target.Name}, dealing {damage:F1} damage!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used High Jump Kick on your {target.Name}, dealing {damage:F1} damage!");
        }
        
    }
}