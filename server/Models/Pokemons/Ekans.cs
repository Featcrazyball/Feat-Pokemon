using Database;
namespace PokemonPocket;

public class Ekans : PokemonMaster
{
    public string? Nickname {get;set;}

    private Ekans() { } //For EF Core
    public Ekans(string nickname, string ownerId) 
    : base("Ekans", "Poison", 35, 60, 44, 40, 54, 55, ownerId, 25, "Bite")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 22) {
            using (var context = new DatabaseContext())
            {
                var arbok = new Arbok(this);
                arbok.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(arbok);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Ekans to a Arbok!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}