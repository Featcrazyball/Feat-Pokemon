using Server;
using PokemonPocket;
using FeatCalculator;

namespace Models;

public class BodySlam : Skill
{
    private BodySlam() { } // For EF Core
    public BodySlam(string PokemonId) : base("Body Slam", "Normal", 85, 1, 15, 1, 0, 0, "The user drops onto the target with its full body weight. It may also leave the target with paralysis.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, float Modifier, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        if (Random.Shared.NextDouble() > Accuracy) {
            await UserSession.SendMessageAsync($"Your {user.Name} used Body Slam, but it missed!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Body Slam, but it missed!");
            return;
        }

        bool paralyze = false;
        if (Random.Shared.NextDouble() <= 0.3) {
            paralyze = true;
            target.Paralyzed = true;
            if (target.ParalyzeSpeed == false) {target.Speed *= 0.5f; target.ParalyzeSpeed = true;}
        }

        bool crit = false;
        if (Random.Shared.NextDouble() <= user.CritRate) {
            crit = true;
        }

        float damage = Calculator.FeatCalculateDamage(BasePower, user.Attack, target.Defense, user.Level, Modifier);
        if (crit) {
            damage *= user.CritDmg;
            await UserSession.SendMessageAsync("CRITICAL HIT!");
            await TargetSession.SendMessageAsync("CRITICAL HIT!");
        }
        
        target.Health -= damage;

        if (paralyze) {
            await UserSession.SendMessageAsync($"Your {user.Name} used Body Slam on {target.Name}, dealing {damage:F1} damage and paralyzing it!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Body Slam on your {target.Name}, dealing {damage:F1} damage and paralyzing it!");
        } else {
            await UserSession.SendMessageAsync($"Your {user.Name} used Body Slam on {target.Name}, dealing {damage:F1} damage.");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Body Slam on your {target.Name}, dealing {damage:F1} damage.");
        }
    }
}