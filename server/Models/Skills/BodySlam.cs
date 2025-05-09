using Server;
using PokemonPocket;

namespace Models;

public class BodySlam : Skill
{
    private BodySlam() { } // For EF Core
    public BodySlam(string PokemonId) : base("Body Slam", "Normal", 85, 1, 15, 1, 0, 0, "The user drops onto the target with its full body weight. It may also leave the target with paralysis.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;

        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Budy Slam") == false)
            return;

        // Damage calculation
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

                bool para;
                if (Random.Shared.NextDouble() <= 0.3) {
                    para = true;
                    target.Paralyzed = para;
                    if (target.ParalyzeSpeed == false) {target.Speed *= 0.5f; target.ParalyzeSpeed = true;}
                    await UserSession.SendMessageAsync($"Your {user.Name} used Body Slam and broke {target.Name}'s Substitude and paralyzing {TargetSession.Username}'s {target.Name}!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Body Slam broke your {target.Name}'s Substitude and paralyzing your {target.Name}!");
                }
                else {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Body Slam and broke {target.Name}'s Substitude!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Body Slam broke your {target.Name}'s Substitude!");
                }
            }
            else
            {
                target.SubstituteHealth -= damage;

                await UserSession.SendMessageAsync($"Your {user.Name} used Body Slam on {target.Name}'s Substitude, dealing {damage} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Body Slam on your {target.Name}'s Substitude, dealing {damage} damage.");
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);

            bool paralyze = false;
            if (Random.Shared.NextDouble() <= 0.3) {
                paralyze = true;
                target.Paralyzed = true;
                if (target.ParalyzeSpeed == false) {target.Speed *= 0.5f; target.ParalyzeSpeed = true;}
            }

            if (paralyze) {
                await UserSession.SendMessageAsync($"Your {user.Name} used Body Slam on {target.Name}, dealing {damage:F1} damage and paralyzing it!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Body Slam on your {target.Name}, dealing {damage:F1} damage and paralyzing it!");
            } else {
                await UserSession.SendMessageAsync($"Your {user.Name} used Body Slam on {target.Name}, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Body Slam on your {target.Name}, dealing {damage:F1} damage.");
            }
        }

    }
}