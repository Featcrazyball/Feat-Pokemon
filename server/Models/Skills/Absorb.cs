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

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, float Modifier, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        float damage = ((user.Level * 2 / 5 + 2) * BasePower * user.Attack / target.Defense / 50 + 2) * Modifier;
        if (damage < 0) damage = 0;

        if (Random.Shared.NextDouble() > user.CritRate) {damage *= user.CritDmg;}
        target.Health -= damage;

        user.Health += damage / 2;
        if (user.Health > user.MaxHealth) user.Health = user.MaxHealth;

        await UserSession.SendMessageAsync($"Your {user.Name} used Absorb on {target.Name}, dealing {damage} damage and recovering {damage / 2} HP.");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Absorb on your {target.Name}, dealing {damage} damage and recovering {damage / 2} HP.");
    }

}
