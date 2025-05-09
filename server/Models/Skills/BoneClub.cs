using Server;
using PokemonPocket;

namespace Models;

public class BoneClub : Skill
{
    private BoneClub() { } // For EF Core
    public BoneClub(string PokemonId) : base("Bone Club", "Ground", 65, 0.85, 20, 1, 0, 0, "The user clubs the target with a bone. It may also make the target flinch.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;

        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Bone Club") == false)
            return;

        // Damage Calculation
        float damage = await SkillHelper.FeatCalculateDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Ground", target.Type?.Split('/') ?? Array.Empty<string>()),  
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

                if (target.Flinch == false || Random.Shared.NextDouble() <= 0.1) 
                {
                    target.Flinch = true;
                    await UserSession.SendMessageAsync($"Your {user.Name} used Bone Club and broke {target.Name}'s Substitude.\n{TargetSession.Username}'s {target.Name} flinched!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Bone Club and broke your {target.Name}'s Substitude.\nYour {target.Name} flinched!");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Bone Club and broke {target.Name}'s Substitude!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Bone Club and broke your {target.Name}'s Substitude!");
                }
            }
            else
            {
                target.SubstituteHealth -= damage;

                await UserSession.SendMessageAsync($"Your {user.Name} used Bone Club on {target.Name}'s Substitude, dealing {damage} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Bone Club on your {target.Name}'s Substitude, dealing {damage} damage.");
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);

            bool flinch = false;
            if (target.Flinch == false || Random.Shared.NextDouble() <= 0.1) 
            {
                flinch = true;
                target.Flinch = true;
            }

            if (flinch) {
                await UserSession.SendMessageAsync($"Your {user.Name} used Bone Club on {target.Name}, dealing {damage:F1} damage.\n{TargetSession.Username}'s {target.Name} flinched!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Bone Club on your {target.Name}, dealing {damage:F1} damage.\nYour {target.Name} flinched!");
            } else {
                await UserSession.SendMessageAsync($"Your {user.Name} used Bone Club on {target.Name}, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Bone Club on your {target.Name}, dealing {damage:F1} damage.");
            }
        }

    }
}