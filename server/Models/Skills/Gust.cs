using Server;
using PokemonPocket;

namespace Models;

public class Gust : Skill
{
    private Gust() { } // For EF Core
    public Gust(string PokemonId) : base("Gust", "Flying", 40, 1, 35, 1, 0, 0, "A gust of wind is whipped up by wings and launched at the target to inflict damage.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Gust") == false)
            return;

        // Damage calculation - double damage against targets using Fly 
        int damageMultiplier = target.Flying ? 2 : 1;
        
        float damage = await SkillHelper.FeatCalculateSpecialDamage(
            BasePower * damageMultiplier, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Flying", target.Type?.Split('/') ?? Array.Empty<string>()),
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
                
                if (target.Underground)
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Gust and broke {target.Name}'s Substitute! It's super effective against the flying target!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Gust and broke your {target.Name}'s Substitute! It's super effective against your flying Pokémon!");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Gust and broke {target.Name}'s Substitute!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Gust and broke your {target.Name}'s Substitute!");
                }
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                if (target.Underground)
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Gust on {target.Name}'s Substitute, dealing {damage:F1} damage! It's super effective against the flying target!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Gust on your {target.Name}'s Substitute, dealing {damage:F1} damage! It's super effective against your flying Pokémon!");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Gust on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Gust on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                }
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            if (target.Underground)
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Gust on {target.Name}, dealing {damage:F1} damage! It's super effective against the flying target!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Gust on your {target.Name}, dealing {damage:F1} damage! It's super effective against your flying Pokémon!");
            }
            else
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Gust on {target.Name}, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Gust on your {target.Name}, dealing {damage:F1} damage.");
            }
        }
        
    }
}