using Server;
using PokemonPocket;
using FeatCalculator;

namespace Models;

public class Blizzard : Skill
{
    private Blizzard() { } // For EF Core
    public Blizzard(string PokemonId) : base("Blizzard", "Ice", 110, 70, 5, 1, 0, 0, "A howling blizzard is summoned to strike the opposing team. It may also freeze them solid.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, float Modifier, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        if (Random.Shared.NextDouble() > Accuracy) {await UserSession.SendMessageAsync($"Your {user.Name} used Bind, but it missed!"); return;}

        if (Random.Shared.NextDouble() > 0.9) {target.Freezing = true;}

        float damage = ((user.Level * 2 / 5 + 2) * BasePower * user.SpecialAttack / target.SpecialDefense / 50 + 2) * Modifier;
        if (Random.Shared.NextDouble() > user.CritRate) 
        {
            damage *= user.CritDmg;
            await UserSession.SendMessageAsync("CRITICAL HIT!");
            await TargetSession.SendMessageAsync("CRITICAL HIT!");
        }
        target.Health -= damage;

        if (target.Freezing)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Blizzard on {target.Name}, dealing {damage} damage and freezing it solid!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Blizzard on your {target.Name}, dealing {damage} damage and freezing it solid!");
        }
        else
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Blizzard on {target.Name}, dealing {damage} damage.");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Blizzard on your {target.Name}, dealing {damage} damage.");
        }
    }
}