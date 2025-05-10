using Server;
using PokemonPocket;

namespace Models;

public class TriAttack : Skill
{
    private TriAttack() { } // For EF Core
    public TriAttack(string PokemonId) : base("Tri Attack", "Normal", 80, 1, 10, 1, 0, 0, "The user strikes with a simultaneous three-beam attack. May also burn, freeze, or leave the target with paralysis.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Tri Attack") == false)
            return;

        // Damage calculation
        float damage = await SkillHelper.FeatCalculateSpecialDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Normal", target.Type?.Split('/') ?? Array.Empty<string>()),  
            UserSession, 
            TargetSession
        );

        bool causedStatus = false;
        string statusEffect = "";
        
        // Substitute handling
        if (target.Substitude)
        {
            if (target.SubstituteHealth <= damage)
            {
                target.Substitude = false;
                target.SubstituteHealth = 0;
                
                if (Random.Shared.NextDouble() <= 0.2)
                {
                    double statusChance = Random.Shared.NextDouble();
                    
                    if (statusChance < 0.33 && (target.Type == null || !target.Type.Contains("Fire")))
                    {
                        // Burn
                        target.Burning = true;
                        target.Attack = (float)(target.Attack * 0.5);
                        statusEffect = "burned";
                        causedStatus = true;
                    }
                    else if (statusChance < 0.67 && (target.Type == null || !target.Type.Contains("Ice")))
                    {
                        // Freeze
                        target.Freezing = true;
                        statusEffect = "frozen";
                        causedStatus = true;
                    }
                    else if (target.Type == null || (!target.Type.Contains("Electric")))
                    {
                        // Paralyze
                        target.Paralyzed = true;
                        if (!target.ParalyzeSpeed)
                        {
                            target.ParalyzeSpeed = true;
                            target.Speed *= 0.5f;
                        }
                        statusEffect = "paralyzed";
                        causedStatus = true;
                    }
                }

                if (causedStatus)
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Tri Attack and broke {target.Name}'s Substitute, causing it to be {statusEffect}!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Tri Attack and broke your {target.Name}'s Substitute, causing it to be {statusEffect}!");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Tri Attack and broke {target.Name}'s Substitute!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Tri Attack and broke your {target.Name}'s Substitute!");
                }

            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Tri Attack on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Tri Attack on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            // 30% chance to cause a status condition - split evenly between burn, freeze, paralyze
            if (Random.Shared.NextDouble() <= 0.2)
            {
                double statusChance = Random.Shared.NextDouble();
                
                if (statusChance < 0.33 && (target.Type == null || !target.Type.Contains("Fire")))
                {
                    // Burn
                    target.Burning = true;
                    target.Attack = (float)(target.Attack * 0.5);
                    statusEffect = "burned";
                    causedStatus = true;
                }
                else if (statusChance < 0.67 && (target.Type == null || !target.Type.Contains("Ice")))
                {
                    // Freeze
                    target.Freezing = true;
                    statusEffect = "frozen";
                    causedStatus = true;
                }
                else if (target.Type == null || (!target.Type.Contains("Electric")))
                {
                    // Paralyze
                    target.Paralyzed = true;
                    if (!target.ParalyzeSpeed)
                    {
                        target.ParalyzeSpeed = true;
                        target.Speed *= 0.5f;
                    }
                    statusEffect = "paralyzed";
                    causedStatus = true;
                }
            }

            if (causedStatus)
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Tri Attack on {target.Name}, dealing {damage:F1} damage! {target.Name} was {statusEffect}!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Tri Attack on your {target.Name}, dealing {damage:F1} damage! Your {target.Name} was {statusEffect}!");
            }
            else
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Tri Attack on {target.Name}, dealing {damage:F1} damage!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Tri Attack on your {target.Name}, dealing {damage:F1} damage!");
            }
        }
    }
}