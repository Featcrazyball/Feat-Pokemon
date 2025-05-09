using Server;
using PokemonPocket;

namespace Models;

public class Constrict : Skill
{
    private Constrict() { } // For EF Core
    public Constrict(string PokemonId) : base("Constrict", "Normal", 10, 1, 35, 1, 0, 0, "The target is attacked with long, creeping tentacles, vines, or the like. It may also lower the target's Speed.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Constrict") == false)
            return;

        bool speedDown = false;

        // Damage calculation
        float damage = await SkillHelper.FeatCalculateDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Normal", target.Type?.Split('/') ?? Array.Empty<string>()),  
            UserSession, 
            TargetSession
        );

        if (target.Substitude)
        {
            if (target.SubstituteHealth <= damage)
            {
                target.Substitude = false;
                target.SubstituteHealth = 0;
                
                if (target.SpeedStage > -6 && Random.Shared.NextDouble() <= 0.33 && !target.Mist)
                {
                    speedDown = true;
                    target.SpeedStage -= 1;
                    target.Speed = target.MaxSpeed * (float)SkillHelper.CalculateStage(target.SpeedStage);
                    if (user.Paralyzed) {user.Speed *= (float)0.5;}
                }

                if (target.Mist)
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Constrict and broke {target.Name}'s Substitute! {target.Name}'s Speed was not lowered due to Mist!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Constrict and broke your {target.Name}'s Substitute! Your {target.Name}'s Speed was not lowered due to Mist!");
                }
                else if (speedDown)
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Constrict and broke {target.Name}'s Substitute! {target.Name}'s Speed was lowered by 1 Stage!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Constrict and broke your {target.Name}'s Substitute! Your {target.Name}'s Speed was lowered by 1 Stage!");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Constrict and broke {target.Name}'s Substitute!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Constrict and broke your {target.Name}'s Substitute!");
                }
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Constrict on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Constrict on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            if (target.SpeedStage > -6 && Random.Shared.NextDouble() <= 0.33 && !target.Mist)
            {
                speedDown = true;
                target.SpeedStage -= 1;
                target.Speed = target.MaxSpeed * (float)SkillHelper.CalculateStage(target.SpeedStage);
                if (user.Paralyzed) {user.Speed *= (float)0.5;}
            }

            if (target.Mist)
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Constrict on {target.Name}, dealing {damage:F1} damage, but its Speed was not lowered due to Mist!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Constrict on your {target.Name}, dealing {damage:F1} damage, but its Speed was not lowered due to Mist!");
            }
            else if (speedDown)
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Constrict on {target.Name}, dealing {damage:F1} damage and lowering its Speed by 1 Stage!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Constrict on your {target.Name}, dealing {damage:F1} damage and lowering its Speed by 1 Stage!");
            }
            else
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Constrict on {target.Name}, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Constrict on your {target.Name}, dealing {damage:F1} damage.");
            }
        }


    }
}