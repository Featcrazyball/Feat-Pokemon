using Server;
using PokemonPocket;

namespace Models;

public class Blizzard : Skill
{
    private Blizzard() { } // For EF Core
    public Blizzard(string PokemonId) : base("Blizzard", "Ice", 110, 0.7, 5, 1, 0, 0, "A howling blizzard is summoned to strike the opposing team. It may also freeze them solid.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;

        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Blizzard") == false)
            return;

        // Damage Calculation
        float damage = await SkillHelper.FeatCalculateDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Ice", target.Type?.Split('/') ?? Array.Empty<string>()),  
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
                if (Random.Shared.NextDouble() > 0.9 && !target.Freezing && !target.Burning) {
                    target.Freezing = true;
                    await UserSession.SendMessageAsync($"Your {user.Name} used Blizzard and broke {target.Name}'s Substitude and freezing {TargetSession.Username}'s {target.Name}!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Blizzard broke your {target.Name}'s Substitude and freezing your {target.Name}!");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Blizzard and broke {target.Name}'s Substitude!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Blizzard broke your {target.Name}'s Substitude!");
                }
            }
            else
            {
                target.SubstituteHealth -= damage;

                await UserSession.SendMessageAsync($"Your {user.Name} used Blizzard on {target.Name}'s Substitude, dealing {damage} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Blizzard on your {target.Name}'s Substitude, dealing {damage} damage.");
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);

            if (Random.Shared.NextDouble() > 0.9 && !target.Freezing && !target.Burning) {target.Freezing = true;}

            if (target.Freezing)
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Blizzard on {target.Name}, dealing {damage} damage and freezing it solid!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Blizzard on your {target.Name}, dealing {damage} damage and freezing it solid!");
            }
            else
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Blizzard on {target.Name}, dealing {damage} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Blizzard on your {target.Name}, dealing {damage} damage.");
            }
        }

    }
}