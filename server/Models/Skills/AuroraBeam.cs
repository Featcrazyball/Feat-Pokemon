using Server;
using PokemonPocket;

namespace Models;

public class AuroraBeam : Skill
{
    private AuroraBeam() { } // For EF Core
    public AuroraBeam(string PokemonId) : base("Aurora Beam", "Ice", 65, 1, 20, 1, 0, 0, "The user attacks with a beam of light that has a chance to lower the target's Attack.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;

        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Aurora Beam") == false)
            return;
        
        // Damage
        float damage = await SkillHelper.FeatCalculateSpecialDamage(
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

                if (target.Mist) {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Aurora Beam on {target.Name}'s Substitude, dealing {damage} damage, but its Attack was not lowered due to Mist!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Aurora Beam on your {target.Name}'s Substitude, dealing {damage} damage, but its Attack was not lowered due to Mist!");
                }
                else if (Random.Shared.NextDouble() < 0.1 && target.AttackStage < 6)
                {
                    target.AttackStage -= 1;
                    target.Attack = (float)(target.MaxAttack * SkillHelper.CalculateStage(target.AttackStage));
                    if (user.Burning) {user.Attack *= (float)0.5;}

                    await UserSession.SendMessageAsync($"Your {user.Name} used Aurora Beam on {target.Name}, dealing {damage} damage and lowering its Attack by 1 Stage.");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Aurora Beam on your {target.Name}, dealing {damage} damage and lowering its Attack by 1 Stage.");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Aurora Beam and broke {target.Name}'s Substitude!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Aurora Beam and broke your {target.Name}'s Substitude!");
                }
            }
            else
            {
                target.SubstituteHealth -= damage;

                await UserSession.SendMessageAsync($"Your {user.Name} used Acid on {target.Name}'s Substitude, dealing {damage} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Acid on your {target.Name}'s Substitude, dealing {damage} damage.");
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);

            bool Mist = false;
            if (Random.Shared.Next(100) < 10 && target.AttackStage < 6)
            {
                if (target.Mist) {
                    Mist = true;
                } else
                {
                    target.AttackStage -= 1;
                    target.Attack = (float)(target.MaxAttack * SkillHelper.CalculateStage(target.AttackStage));
                    if (target.Burning) {target.Attack *= (float)0.5;}
                }

                if (Mist)
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Aurora Beam on {target.Name}, dealing {damage} damage, but its Attack was not lowered due to Mist!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Aurora Beam on your {target.Name}, dealing {damage} damage, but its Attack was not lowered due to Mist!");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Aurora Beam on {target.Name}, dealing {damage} damage and lowering its Attack by 1 Stage.");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Aurora Beam on your {target.Name}, dealing {damage} damage and lowering its Attack by 1 Stage.");
                }
            }
            else
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Aurora Beam on {target.Name}, dealing {damage} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Aurora Beam on your {target.Name}, dealing {damage} damage.");
            }
        }

    }
}