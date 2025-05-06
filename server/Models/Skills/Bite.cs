using Server;
using PokemonPocket;
using FeatCalculator;

namespace Models;

public class Bite : Skill
{
    private Bite() { } // For EF Core
    public Bite(string PokemonId) : base("Bite", "Dark", 60, 1, 25, 1, 0, 0, "The user bites the target. It may cause flinching.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, float Modifier, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;

        if (target.Flinch == false)
            if (Random.Shared.NextDouble() > 0.9) {target.Flinch = true; target.FlinchTurns =1;}

        float damage = ((user.Level * 2 / 5 + 2) * BasePower * user.Attack / target.Defense / 50 + 2) * Modifier;
        if (Random.Shared.NextDouble() > user.CritRate) 
        {
            damage *= user.CritDmg;
            await UserSession.SendMessageAsync("CRITICAL HIT!");
            await TargetSession.SendMessageAsync("CRITICAL HIT!");
        }
        if (damage < 0) damage = 0;

        target.Health -= damage;

        if (target.Flinch)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Bite on {target.Name}, dealing {damage} damage and causing flinching!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Bite on your {target.Name}, dealing {damage} damage and causing flinching!");
        }
        else
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Bite on {target.Name}, dealing {damage} damage.");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Bite on your {target.Name}, dealing {damage} damage.");
        }
    }
}