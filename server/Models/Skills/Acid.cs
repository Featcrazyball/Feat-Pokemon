using Server;
using PokemonPocket;

namespace Models;

public class Acid : Skill
{
    private Acid() { } // For EF Core
    public Acid(string PokemonId) : base("Acid", "Poison", 40, 1, 30, 1, 0, 0, "The user spews a vile liquid that may lower the target's Special Defense.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;

        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Acid") == false) {return;}
        
        float damage = await SkillHelper.FeatCalculateDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Poison", target.Type?.Split('/') ?? Array.Empty<string>()),  
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

                bool decrease = false;
                bool Mist = false;
                if (Random.Shared.NextDouble() <= 0.10)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        if (target.SpecialDefenseStage >= 6) {break;}
                        if (target.Mist) {Mist = true; break;}
                        decrease = true;
                        target.SpecialDefenseStage -= 1;
                        target.SpecialDefense = target.MaxSpecialDefense * (float)SkillHelper.CalculateStage(target.SpecialDefenseStage); 
                    }
                }

                if (Mist)
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Acid on {target.Name}'s Substitude, dealing {damage} damage, but {target.SpecialDefense} was not lowered due to Mist!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Acid on your {target.Name}'s Substitude, dealing {damage} damage, but {target.SpecialDefense} was not lowered due to Mist!!");
                }
                else if (decrease)
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Acid on {target.Name}'s Substitude, dealing {damage} damage and reducing {target.Name}'s Special Defense by 1 Stage.");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Acid on your {target.Name}'s Substitude, dealing {damage} damage reducing {target.Name}'s Special Defense by 1 Stage.");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Acid and broke {target.Name}'s Substitude!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Acid and broke your {target.Name}'s Substitude!");
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
            bool decrease = false;
            if (Random.Shared.NextDouble() <= 0.10)
            {
                for (int i = 0; i < 1; i++)
                {
                    if (target.SpecialDefenseStage >= 6) {decrease=true;break;}
                    if (target.Mist) {Mist = true; break;}
                    target.SpecialDefenseStage -= 1;
                    target.SpecialDefense = target.MaxSpecialDefense * (float)SkillHelper.CalculateStage(target.SpecialDefenseStage); 
                }
            }

            if (Mist)
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Acid on {target.Name}, dealing {damage} damage, but {target.SpecialDefense} was not lowered due to Mist!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Acid on your {target.Name}, dealing {damage} damage, but {target.SpecialDefense} was not lowered due to Mist!!");
                return;
            }
            else if (decrease)
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Acid on {target.Name}, dealing {damage} damage and lowering its Special Defense by 1 Stage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Acid on your {target.Name}, dealing {damage} damage and lowering its Special Defense by 1 Stage.");
            }
            else
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Acid on {target.Name}, dealing {damage} damage. However, Special Defense was not reduced.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Acid on your {target.Name}, dealing {damage} damage. However, Special Defense was not reduced.");
            }
        }

    }
}