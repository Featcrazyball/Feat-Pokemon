using Database;
namespace PokemonPocket;

public class Zubat : PokemonMaster
{
    public string? Nickname {get;set;}

    private Zubat() { } //For EF Core
    public Zubat(string nickname, string ownerId) 
    : base("Zubat", "Poison/Flying", 40, 45, 40, 30, 40, 55, ownerId, 10, "Inner Focus")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 22) {
            using (var context = new DatabaseContext())
            {
                var golbat = new Golbat(this);
                golbat.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(golbat);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Zubat to a Golbat!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}