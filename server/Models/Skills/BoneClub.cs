using Server;
using PokemonPocket;
using FeatCalculator;

namespace Models;

public class BoneClub : Skill
{
    private BoneClub() { } // For EF Core
    public BoneClub(string PokemonId) : base("Bone Club", "Ground", 65, 0.85, 20, 1, 0, 0, "The user clubs the target with a bone. It may also make the target flinch.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, float Modifier, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        if (Random.Shared.NextDouble() > Accuracy) {
            await UserSession.SendMessageAsync($"Your {user.Name} used Bone Club, but it missed!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Bone Club, but it missed!");
            return;
        }

        bool flinch = false;
        if (target.Flinch == false) {
            if (Random.Shared.NextDouble() <= 0.1) {
            flinch = true;
            target.Flinch = true;
        }
        }

        bool crit = false;
        if (Random.Shared.NextDouble() <= user.CritRate) {
            crit = true;
            await UserSession.SendMessageAsync("CRITICAL HIT!");
            await TargetSession.SendMessageAsync("CRITICAL HIT!");
        }

        float damage = Calculator.FeatCalculateDamage(BasePower, user.Attack, target.Defense, user.Level, Modifier);
        if (crit) {
            damage *= user.CritDmg;
        }
        
        target.Health -= damage;

        if (flinch) {
            await UserSession.SendMessageAsync($"Your {user.Name} used Bone Club on {target.Name}, dealing {damage:F1} damage and causing it to flinch!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Bone Club on your {target.Name}, dealing {damage:F1} damage and causing it to flinch!");
        } else {
            await UserSession.SendMessageAsync($"Your {user.Name} used Bone Club on {target.Name}, dealing {damage:F1} damage.");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Bone Club on your {target.Name}, dealing {damage:F1} damage.");
        }
    }
}