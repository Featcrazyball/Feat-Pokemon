using Database;
namespace PokemonPocket;

public class Sandshrew : PokemonMaster
{
    public string? Nickname {get;set;}

    private Sandshrew() { } //For EF Core
    public Sandshrew(string nickname, string ownerId) 
    : base("Sandshrew", "Ground", 50, 75, 85, 20, 30, 40, ownerId, 25, "Scratch")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 22) {
            using (var context = new DatabaseContext())
            {
                var sandslash = new Sandslash(this);
                sandslash.EvolveLevelUp(Level-1); // Level up to 22

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(sandslash);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Sandshrew to a Sandslash!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}