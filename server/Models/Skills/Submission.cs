using Server;
using PokemonPocket;

namespace Models;

public class Submission : Skill
{
    private Submission() { } // For EF Core
    public Submission(string PokemonId) : base("Submission", "Fighting", 80, 0.8, 20, 1, 0, 0, "The user grabs the target and recklessly dives for the ground. This also damages the user.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Submission") == false)
            return;

        // Recoil damage (1/4 of damage dealt in Gen 1)
        float recoilDamage;

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
                recoilDamage = target.SubstituteHealth;
                user.Health -= recoilDamage;
                target.SubstituteHealth = 0;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Submission and broke {target.Name}'s Substitute!");
                await UserSession.SendMessageAsync($"Your {user.Name} was hurt by recoil, taking {recoilDamage:F1} damage!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Submission and broke your {target.Name}'s Substitute!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} was hurt by recoil, taking {recoilDamage:F1} damage!");
            }
            else
            {
                target.SubstituteHealth -= damage;
                recoilDamage = damage / 4;
                user.Health -= recoilDamage; 
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Submission on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await UserSession.SendMessageAsync($"Your {user.Name} was hurt by recoil, taking {recoilDamage:F1} damage!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Submission on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} was hurt by recoil, taking {recoilDamage:F1} damage!");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }

        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            // Recoil damage (1/4 of damage dealt in Gen 1)
            recoilDamage = damage / 4;
            user.Health -= recoilDamage;
            if (user.Health < 0) user.Health = 0;
            
            await UserSession.SendMessageAsync($"Your {user.Name} used Submission on {target.Name}, dealing {damage:F1} damage!\nYour {user.Name} was hurt by recoil, taking {recoilDamage:F1} damage!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Submission on your {target.Name}, dealing {damage:F1} damage!\n{UserSession.Username}'s {user.Name} was hurt by recoil!");
        }
    }
}