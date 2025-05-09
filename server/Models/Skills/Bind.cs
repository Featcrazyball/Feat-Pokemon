using Server;
using PokemonPocket;

namespace Models;

public class Bind : Skill
{
    private Bind() { } // For EF Core
    public Bind(string PokemonId) : base("Bind", "Normal", 15, 0.85, 20, 1, 0, 0, "The user wraps its body around the target and squeezes it for two to five turns.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;

        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Bind") == false)
            return;

        int turns;
        float chance = Random.Shared.Next(0, 100);
        if (chance > 87.5) turns = 5;
        else if (chance > 75) turns = 4;
        else if (chance > 37.5) turns = 3;
        else turns = 2;

        // Damage Calculation
        float damage = await SkillHelper.FeatCalculateDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Normal", target.Type?.Split('/') ?? Array.Empty<string>()),  
            UserSession, 
            TargetSession
        );

        if (target.Substitude == true)
        {
            if (target.SubstituteHealth <= damage) 
            {
                target.Substitude = false;
                target.SubstituteHealth = 0;

                if (!target.BindActive) {
                    target.BindDamage = target.MaxHealth / 8; 
                    target.BindTurns = turns;
                    target.BindActive = true;
                }

                await UserSession.SendMessageAsync($"Your {user.Name} used Bind and broke {target.Name}'s Substitude and binding {TargetSession.Username}'s {target.Name}!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Bind broke your {target.Name}'s Substitude and bind your {target.Name}!");
            }
            else
            {
                target.SubstituteHealth -= damage;

                await UserSession.SendMessageAsync($"Your {user.Name} used Bind on {target.Name}'s Substitude, dealing {damage} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Bind on your {target.Name}'s Substitude, dealing {damage} damage.");
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);

            if (!target.BindActive) {
                target.BindDamage = target.MaxHealth / 8; 
                target.BindTurns = turns;
                target.BindActive = true;
            }

            await UserSession.SendMessageAsync($"Your {user.Name} used Bind and is now squeezing the target!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Bind and is now squeezing the target!");
        }
    }
}