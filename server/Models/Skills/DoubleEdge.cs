using Server;
using PokemonPocket;

namespace Models;

public class DoubleEdge : Skill
{
    private DoubleEdge() { } // For EF Core
    public DoubleEdge(string PokemonId) : base("Double Edge", "Normal", 120, 1, 15, 1, 0, 0, "The user charges at the target and attacks. The user also takes damage equal to half the damage dealt to the target.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;

        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);
        
        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Double Edge") == false)
            return;

        // Damage Calculation
        float damage = await SkillHelper.FeatCalculateDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Normal", target.Type?.Split('/') ?? Array.Empty<string>()),  
            UserSession, 
            TargetSession
        );

        // Substitude
        if (target.Substitude == true)
        {
            if (target.SubstituteHealth <= damage) 
            {
                target.Substitude = false;
                target.SubstituteHealth = 0;

                await UserSession.SendMessageAsync($"Your {user.Name} used Double Edge and broke {target.Name}'s Substitude!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Double Edge broke your {target.Name}'s Substitude!");
            }
            else
            {
                target.SubstituteHealth -= damage;

                // Recoil damage
                float recoilDamage = damage / 2;
                user.Health -= recoilDamage;
                if (user.Health < 0) user.Health = 0;

                await UserSession.SendMessageAsync($"Your {user.Name} used Double Edge on {target.Name}'s Substitude, dealing {damage} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Double Edge on your {target.Name}'s Substitude, dealing {damage} damage.");
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);

            // Recoil damage
            float recoilDamage = damage / 2;
            user.Health -= recoilDamage;
            if (user.Health < 0) user.Health = 0;
        
            await UserSession.SendMessageAsync($"Your {user.Name} used Double Edge on {target.Name}, dealing {damage:F1} damage!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Double Edge on your {target.Name}, dealing {damage:F1} damage!");
            await UserSession.SendMessageAsync($"Your {user.Name} took recoil damage of {recoilDamage:F1} from using Double Edge!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} took recoil damage of {recoilDamage:F1} from using Double Edge!");
        }

    }
}