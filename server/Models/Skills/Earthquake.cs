using Server;
using PokemonPocket;

namespace Models;

public class Earthquake : Skill
{
    private Earthquake() { } // For EF Core
    public Earthquake(string PokemonId) : base("Earthquake", "Ground", 100, 1, 10, 1, 0, 0, "A powerful quake that deals damage and is twice as powerful against opponents using Dig.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Earthquake") == false)
            return;

        // Check if target is underground (using Dig)
        int Power = target.Underground ? 200 : 100;
        
        // Damage calculation
        float damage = await SkillHelper.FeatCalculateDamage(
            Power, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Ground", target.Type?.Split('/') ?? Array.Empty<string>()),  
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
                    await UserSession.SendMessageAsync($"Your {user.Name} used Earthquake and broke {target.Name}'s Substitute! It's super effective against the underground target!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Earthquake and broke your {target.Name}'s Substitute! It's super effective against your underground Pokémon!");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Earthquake and broke {target.Name}'s Substitute!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Earthquake and broke your {target.Name}'s Substitute!");
                }
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                if (target.Underground)
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Earthquake on {target.Name}'s Substitute, dealing {damage:F1} damage! It's super effective against the underground target!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Earthquake on your {target.Name}'s Substitute, dealing {damage:F1} damage! It's super effective against your underground Pokémon!");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Earthquake on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Earthquake on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
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
                await UserSession.SendMessageAsync($"Your {user.Name} used Earthquake on {target.Name}, dealing {damage:F1} damage! It's super effective against the underground target!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Earthquake on your {target.Name}, dealing {damage:F1} damage! It's super effective against your underground Pokémon!");
            }
            else
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Earthquake on {target.Name}, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Earthquake on your {target.Name}, dealing {damage:F1} damage.");
            }
        }
        
    }
}