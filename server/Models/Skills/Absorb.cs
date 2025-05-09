using PokemonPocket;
using Server;

namespace Models;

public class Absorb : Skill
{
    private Absorb() { } // For EF Core
    public Absorb(string PokemonId) : base("Absorb", "Grass", 20, 1, 25, 1, 0, 0, "Absorb the target's HP and restore your own.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Absorb") == false) {return;}

        // Damage calculation
        float damage = await SkillHelper.FeatCalculateDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Grass", target.Type?.Split('/') ?? Array.Empty<string>()),  
            UserSession, 
            TargetSession
        );

        // Substitude 
        if (target.Substitude == true && target.SubstituteHealth > 0)
        {
            bool broken = false;

            if (target.SubstituteHealth > damage) 
            {
                target.SubstituteHealth -= damage;
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
            else if (target.SubstituteHealth <= damage) 
            {
                target.SubstituteHealth = 0;

                broken = true;
            }
            
            if (broken) 
            {
                target.Substitude = false;
                await UserSession.SendMessageAsync($"Your {user.Name} broke {target.Name}'s Substitude!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} broke your {target.Name}'s Substitude!");
            }
            else if (!broken)
            {
                await UserSession.SendMessageAsync($"Your {user.Name} used Absorb on {target.Name}'s Substitude, dealing {damage} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Absorb on your {target.Name}'s Substitude, dealing {damage} damage.");
            }
            return;
        }
        else
        {
            if (target.Health < damage) 
            {
                user.Health += target.Health / 2;
                if (user.Health > user.MaxHealth) user.Health = user.MaxHealth;
            }
            else
            {
                user.Health += damage / 2;
                if (user.Health > user.MaxHealth) user.Health = user.MaxHealth;
            }
                
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);

            if (user.Health > user.MaxHealth) {user.Health = user.MaxHealth;}

            await UserSession.SendMessageAsync($"Your {user.Name} used Absorb on {target.Name}, dealing {damage} damage and recovering {damage / 2} HP.");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Absorb on your {target.Name}, dealing {damage} damage and recovering {damage / 2} HP.");
        }

    }
}
