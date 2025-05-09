using Server;
using PokemonPocket;

namespace Models;

public class BubbleBeam : Skill
{
    private BubbleBeam() { } // For EF Core
    public BubbleBeam(string PokemonId) : base("Bubble Beam", "Water", 65, 1, 20, 1, 0, 0, "A spray of bubbles is forcefully ejected at the target. It may also lower the target's Speed.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;

        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Bubble Beam") == false)
            return;

        bool speedDown = false;

        float damage = await SkillHelper.FeatCalculateSpecialDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Water", target.Type?.Split('/') ?? Array.Empty<string>()),  
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
                        if (target.SpeedStage >= 6) {break;}
                        if (target.Mist) {Mist = true; break;}
                        decrease = true;
                        target.SpeedStage -= 1;
                        target.Speed = target.MaxSpeed * (float)SkillHelper.CalculateStage(target.SpeedStage); 
                    }
                    if (user.Paralyzed) {user.Speed *= (float)0.5;}
                }

                if (Mist)
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Bubble Beam on {target.Name}'s Substitude, dealing {damage} damage, but {target.Speed} was not lowered due to Mist!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Bubble Beam on your {target.Name}'s Substitude, dealing {damage} damage, but {target.Speed} was not lowered due to Mist!");
                }
                else if (decrease)
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Bubble Beam on {target.Name}'s Substitude, dealing {damage} damage and lowering {target.Name}'s Speed by 1 Stage.");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Bubble Beam on your {target.Name}'s Substitude, dealing {damage} damage and lowering {target.Name}'s Speed by 1 Stage.");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Bubble Beam and broke {target.Name}'s Substitude!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Bubble Beam and broke your {target.Name}'s Substitude!");
                }
            }
            else
            {
                target.SubstituteHealth -= damage;

                await UserSession.SendMessageAsync($"Your {user.Name} used Bubble Beam on {target.Name}'s Substitude, dealing {damage} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Bubble Beam on your {target.Name}'s Substitude, dealing {damage} damage.");
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);

            bool Mist = false;
            if (Random.Shared.NextDouble() <= 0.10)
            {
                for (int i = 0; i < 1; i++)
                {
                    if (target.SpeedStage >= 6) {break;}
                    if (target.Mist) {Mist = true; break;}
                    speedDown = true;
                    target.SpeedStage -= 1;
                    target.Speed = target.MaxSpeed * (float)SkillHelper.CalculateStage(target.SpeedStage); 
                }
                if (user.Paralyzed) {user.Speed *= (float)0.5;}
            }

            if (Mist)
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Bubble Beam on {target.Name}, dealing {damage:F1} damage, but its Speed was not lowered due to Mist!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Bubble Beam on your {target.Name}, dealing {damage:F1} damage, but its Speed was not lowered due to Mist!");
            }
            else if (speedDown) {
                await UserSession.SendMessageAsync($"Your {user.Name} used Bubble Beam on {target.Name}, dealing {damage:F1} damage and lowering its Speed by 1 Stage!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Bubble Beam on your {target.Name}, dealing {damage:F1} damage and lowering its Speed by 1 Stage!");
            } else {
                await UserSession.SendMessageAsync($"Your {user.Name} used Bubble Beam on {target.Name}, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Bubble Beam on your {target.Name}, dealing {damage:F1} damage.");
            }
        }

    }
}