using Server;
using PokemonPocket;

namespace Models;

public class Bite : Skill
{
    private Bite() { } // For EF Core
    public Bite(string PokemonId) : base("Bite", "Dark", 60, 1, 25, 1, 0, 0, "The user bites the target. It may cause flinching.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;

        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Bite") == false)
            return;

        // Damage Calculation
        float damage = await SkillHelper.FeatCalculateSpecialDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Dark", target.Type?.Split('/') ?? Array.Empty<string>()),  
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

                if (target.Flinch == false || Random.Shared.NextDouble() > 0.9)
                {
                    target.Flinch = true; 
                    await UserSession.SendMessageAsync($"Your {user.Name} used Bite and broke {target.Name}'s Substitude.\n{TargetSession.Username}'s {target.Name} flinched!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Bite broke your {target.Name}'s Substitude.\nYour {target.Name} flinched!");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Bite and broke {target.Name}'s Substitude!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Bite broke your {target.Name}'s Substitude!");
                }
            }
            else
            {
                target.SubstituteHealth -= damage;

                await UserSession.SendMessageAsync($"Your {user.Name} used Bite on {target.Name}'s Substitude, dealing {damage} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Bite on your {target.Name}'s Substitude, dealing {damage} damage.");
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);

            if (target.Flinch == false || Random.Shared.NextDouble() > 0.9)
            {
                target.Flinch = true; 
            }

            if (target.Flinch)
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Bite on {target.Name}, dealing {damage} damage.\n{TargetSession.Username}'s {target.Name} flinched!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Bite on your {target.Name}, dealing {damage} damage.\nYour {target.Name} flinched!");
            }
            else
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Bite on {target.Name}, dealing {damage} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Bite on your {target.Name}, dealing {damage} damage.");
            }
        }

    }
}