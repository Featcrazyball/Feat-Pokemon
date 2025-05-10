using Server;
using PokemonPocket;

namespace Models;

public class Thunder : Skill
{
    private Thunder() { } // For EF Core
    public Thunder(string PokemonId) : base("Thunder", "Electric", 110, 0.7, 10, 1, 0, 0, "A wicked thunderbolt is dropped on the target to inflict damage. It may also leave the target with paralysis.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName: "Thunder") == false)
            return;

        // Damage calculation
        float damage = await SkillHelper.FeatCalculateSpecialDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Electric", target.Type?.Split('/') ?? Array.Empty<string>()),  
            UserSession, 
            TargetSession
        );

        bool causedParalysis = false;
        
        // Substitute handling
        if (target.Substitude)
        {
            if (target.SubstituteHealth <= damage)
            {
                target.Substitude = false;
                target.SubstituteHealth = 0;
                
                // 10% chance to paralyze in Gen 1
                if (!target.Paralyzed && Random.Shared.NextDouble() <= 0.3)
                {
                    target.Paralyzed = true;
                    if (!target.ParalyzeSpeed)
                    {
                        target.ParalyzeSpeed = true;
                        target.Speed *= 0.5f;
                    }
                    causedParalysis = true;
                }

                if (causedParalysis)
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Thunder and broke {target.Name}'s Substitute!\nnIt also paralyzed {TargetSession.Username} {target.Name}!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Thunder and broke your {target.Name}'s Substitute!\nYour {target.Name} is now paralyzed!");
                }
                else
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Thunder and broke {target.Name}'s Substitute!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Thunder and broke your {target.Name}'s Substitute!");
                }

            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Thunder on {target.Name}'s Substitute, dealing {damage:F1} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Thunder on your {target.Name}'s Substitute, dealing {damage:F1} damage.");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            // 10% chance to paralyze in Gen 1
            if (!target.Paralyzed && Random.Shared.NextDouble() <= 0.3)
            {
                target.Paralyzed = true;
                if (!target.ParalyzeSpeed)
                {
                    target.ParalyzeSpeed = true;
                    target.Speed *= 0.5f;
                }
                causedParalysis = true;
            }

            if (causedParalysis)
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Thunder on {target.Name}, dealing {damage:F1} damage!\nIt also paralyzed {TargetSession.Username} {target.Name}!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Thunder on your {target.Name}, dealing {damage:F1} damage!\nYour {target.Name} is now paralyzed!");
            }
            else
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Thunder on {target.Name}, dealing {damage:F1} damage!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Thunder on your {target.Name}, dealing {damage:F1} damage!");
            }
        }
    }
}