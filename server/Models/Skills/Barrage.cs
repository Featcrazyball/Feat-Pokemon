using Server;
using PokemonPocket;
using FeatCalculator;
using System.Runtime.ConstrainedExecution;

namespace Models;

public class Barrage : Skill
{
    private Barrage() { } // For EF Core
    public Barrage(string PokemonId) : base("Barrage", "Normal", 15, 0.85, 20, 1, 0, 0, "The user attacks the target with a barrage of punches. It may hit two to five times in one turn.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, float Modifier, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        // Accuracy check
        if (Random.Shared.NextDouble() > Accuracy)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Barrage, but it missed!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Barrage, but it missed!");
            return;
        }
        bool crit = false;
        if (Random.Shared.NextDouble() > user.CritRate) {crit = true;}
        if (crit) 
        {
            await UserSession.SendMessageAsync("CRITICAL HIT!"); 
            await TargetSession.SendMessageAsync("CRITICAL HIT!");
        }

        int hits;
        float chance = Random.Shared.Next(0, 100);
        if (chance > 87.5) hits = 5;
        else if (chance > 75) hits = 4;
        else if (chance > 37.5) hits = 3;
        else hits = 2;

        float totalDamage = 0;

        for (int i = 0; i < hits; i++)
        {
            float damage = ((user.Level * 2 / 5 + 2) * BasePower*(i+1) * user.Attack / target.Defense / 50 + 2) * Modifier;
            if (damage < 0) damage = 0;
            if (crit) {damage *= user.CritDmg;}
            totalDamage += damage;
            target.Health -= damage;
        }

        await UserSession.SendMessageAsync($"Your {user.Name} used Barrage on {target.Name}, hitting {hits} times for a total of {totalDamage} damage.");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Barrage on your {target.Name}, hitting {hits} times for a total of {totalDamage} damage.");
    }
}