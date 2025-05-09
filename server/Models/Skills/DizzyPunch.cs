using Server;
using PokemonPocket;

namespace Models;

public class DizzyPunch : Skill
{
    private DizzyPunch() { } // For EF Core
    public DizzyPunch(string PokemonId) : base("Dizzy Punch", "Normal", 70, 1, 10, 1, 0, 0, "The user throws a punch in a dizzying fashion. It may cause confusion.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);
        
        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Dizzy Punch") == false)
            return;

        // Damage Calculation
        float damage = await SkillHelper.FeatCalculateDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Normal", target.Type?.Split('/') ?? Array.Empty<string>()),  
            UserSession, 
            TargetSession
        );

        int hits;
        float chance = Random.Shared.Next(0, 100);
        if (chance > 87.5) hits = 5;
        else if (chance > 75) hits = 4;
        else if (chance > 37.5) hits = 3;
        else hits = 2;

        // Substitute
        if (target.Substitude == true)
        {
            if (target.SubstituteHealth <= damage) 
            {
                target.Substitude = false;
                target.SubstituteHealth = 0;

                if (target.Confused == false)
                {
                    if (Random.Shared.NextDouble() > 0.9) {target.Confused = true; target.ConfusionTurns = hits;}
                    await UserSession.SendMessageAsync($"Your {user.Name} used Dizzy Punch and broke {target.Name}'s Substitude and confusing {TargetSession.Username}'s {target.Name} for {hits} turn(s)!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Dizzy Punch broke your {target.Name}'s Substitude and confusing your {target.Name} for {hits} turn(s)!");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Dizzy Punch and broke {target.Name}'s Substitude!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Dizzy Punch broke your {target.Name}'s Substitude!");
                }
            }
            else
            {
                target.SubstituteHealth -= damage;

                await UserSession.SendMessageAsync($"Your {user.Name} used Dizzy Punch on {target.Name}'s Substitude, dealing {damage} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Dizzy Punch on your {target.Name}'s Substitude, dealing {damage} damage.");
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);

            bool confuse = false;
            if (target.Confused == false) 
            {
                if (Random.Shared.NextDouble() <= 0.10) 
                confuse = true;
            }

            await UserSession.SendMessageAsync($"Your {user.Name} used Dizzy Punch on {target.Name}, dealing {damage:F1} damage.");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Dizzy Punch on your {target.Name}, dealing {damage:F1} damage.");

            if (confuse)
            {
                target.Confused = true;
                target.ConfusionTurns = hits;
                await UserSession.SendMessageAsync($"{target.Name} became confused for {hits} turn(s)!");
                await TargetSession.SendMessageAsync($"Your {target.Name} became confused for {hits} turn(s)!");
            }
        }


    }
}
