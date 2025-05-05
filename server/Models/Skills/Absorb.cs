using PokemonPocket;
namespace Models;

public class Absorb : Skill
{
    private Absorb() { } // For EF Core
    public Absorb(string PokemonId) : base("Absorb", "Grass", 20, 1, 1, 25, 0, 0, "Absorb the target's HP and restore your own.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override void SkillEfect(PokemonMaster target, PokemonMaster user, float Modifier)
    {
        float damage = ((user.Level * 2 / 5 + 2) * BasePower * user.Attack / target.Defense / 50 + 2) * Modifier;
        if (damage < 0) damage = 0;

        target.Health -= damage;

        user.Health += damage / 2;
        if (user.Health > user.MaxHealth) user.Health = user.MaxHealth;

        Console.WriteLine($"{user.Name} used {Name} on {target.Name}, dealing {damage} damage and recovering {damage / 2} HP.");
    }

}
