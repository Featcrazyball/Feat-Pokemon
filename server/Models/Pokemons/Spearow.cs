using Database;
namespace PokemonPocket;

public class Spearow : PokemonMaster
{
    public string? Nickname {get;set;}

    private Spearow() { } //For EF Core
    public Spearow(string nickname, string ownerId) 
    : base("Spearow", "Normal/Flying", 40, 60, 30, 31, 31, 70, ownerId, 25, "Peck")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 20) {
            using (var context = new DatabaseContext())
            {
                var fearow = new Fearow(this);
                fearow.EvolveLevelUp(Level-1); // Level up to 20

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(fearow);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Spearow to a Fearow!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return 3*SkillDamage;
    }
}