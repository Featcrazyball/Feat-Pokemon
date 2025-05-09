using Server;
using PokemonPocket;

namespace Models;

public class FireSpin : Skill
{
    private FireSpin() { } // For EF Core
    public FireSpin(string PokemonId) : base("Fire Spin", "Fire", 35, 0.85, 15, 1, 0, 0, "The target becomes trapped within a fierce vortex of fire that rages for four to five turns.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Fire Spin") == false)
            return;

        int hits;
        float chance = (float)Random.Shared.NextDouble();
        if (chance > 0.875) hits = 5;
        else if (chance >0.75) hits = 4;
        else if (chance > 0.375) hits = 3;
        else hits = 2;

        // Damage calculation
        float damage = await SkillHelper.FeatCalculateSpecialDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Fire", target.Type?.Split('/') ?? Array.Empty<string>()),  
            UserSession, 
            TargetSession
        );

        // Substitute handling
        if (target.Substitude)
        {
            if (target.SubstituteHealth <= damage)
            {
                target.Substitude = false;
                target.SubstituteHealth = 0;
                
                // Bind
                if (!target.BindActive) {
                    target.BindDamage = target.MaxHealth / 8; 
                    target.BindTurns = hits;
                    target.BindActive = true;
                }

                await UserSession.SendMessageAsync($"Your {user.Name} used Fire Spin and broke {target.Name}'s Substitute! {target.Name} is trapped in a vortex of flame!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Fire Spin and broke your {target.Name}'s Substitute! Your {target.Name} is trapped in a vortex of flame!");
            }
            else
            {
                target.SubstituteHealth -= damage;
                
                await UserSession.SendMessageAsync($"Your {user.Name} used Fire Spin on {target.Name}'s Substitute, dealing {damage:F1} damage. {target.Name} is trapped in a vortex of flame!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Fire Spin on your {target.Name}'s Substitute, dealing {damage:F1} damage. Your {target.Name} is trapped in a vortex of flame!");
                
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        }
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            
            // Bind
            if (!target.BindActive) {
                target.BindDamage = target.MaxHealth / 8; 
                target.BindTurns = hits;
                target.BindActive = true;
            }

            await UserSession.SendMessageAsync($"Your {user.Name} used Fire Spin on {target.Name}, dealing {damage:F1} damage. {target.Name} is trapped in a vortex of flame!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Fire Spin on your {target.Name}, dealing {damage:F1} damage. Your {target.Name} is trapped in a vortex of flame!");
        }
    }
}