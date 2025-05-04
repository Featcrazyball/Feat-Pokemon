using Database;
namespace PokemonPocket;

public class Drowzee : PokemonMaster
{
    public string? Nickname {get;set;}

    private Drowzee() { } //For EF Core
    public Drowzee(string nickname, string ownerId) 
    : base("Drowzee", "Psychic", 60, 48, 45, 43, 90, 42, ownerId, 20, "Insomnia")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 26) {
            using (var context = new DatabaseContext())
            {
                var hypno = new Hypno(this);
                hypno.EvolveLevelUp(Level-1); // Level up to current level

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(hypno);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Drowzee to a Hypno!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}