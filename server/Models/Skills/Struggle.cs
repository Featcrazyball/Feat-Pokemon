using Server;
using PokemonPocket;

namespace Models;

public class Struggle : Skill
{
    private Struggle() { } // For EF Core
    public Struggle(string PokemonId) : base("Struggle", "Normal", 50, 1, -1, 1, 0, 0, "A desperate attack used only if the Pokémon has no PP. It also hurts the user a little.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // recooil damage
        float recoil;

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
                recoil = target.SubstituteHealth*0.25f;
                user.Health -= recoil;
                target.SubstituteHealth = 0;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Struggle and broke {target.Name}'s Substitute!");
                await UserSession.SendMessageAsync($"Your {user.Name} was hit with recoil, taking {recoil:F1} damage!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Struggle and broke your {target.Name}'s Substitute!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} was hit with recoil, taking {recoil:F1} damage!");
            }
            else
            {
                target.SubstituteHealth -= damage;
                recoil = damage*0.25f;
                user.Health -= recoil;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Struggle on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await UserSession.SendMessageAsync($"Your {user.Name} was hit with recoil, taking {recoil:F1} damage!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Struggle on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} was hit with recoil, taking {recoil:F1} damage!");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            recoil = damage * 0.25f; 
            user.Health -= recoil;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            await UserSession.SendMessageAsync($"Your {user.Name} used Struggle on {target.Name}, dealing {damage:F1} damage!\nYour {user.Name} was hit with recoil, taking {recoil:F1} damage!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Struggle on your {target.Name}, dealing {damage:F1} damage!\n{UserSession.Username}'s {user.Name} was hit with recoil of {recoil:F1} damage!");
        }
    }
}