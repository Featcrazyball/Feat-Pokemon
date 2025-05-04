using Database;
namespace PokemonPocket;

public class Poliwag : PokemonMaster
{
    public string? Nickname {get;set;}

    private Poliwag() { } //For EF Core
    public Poliwag(string nickname, string ownerId) 
    : base("Poliwag", "Water", 40, 50, 40, 40, 40, 90, ownerId, 16, "Water Absorb")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 25) {
            using (var context = new DatabaseContext())
            {
                var poliwhirl = new Poliwhirl(this);
                poliwhirl.EvolveLevelUp(Level-1); 

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(poliwhirl);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Poliwag to a Poliwhirl!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}