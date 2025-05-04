using Database;
namespace PokemonPocket;

public class Koffing : PokemonMaster
{
    public string? Nickname {get;set;}

    private Koffing() { } //For EF Core
    public Koffing(string nickname, string ownerId) 
    : base("Koffing", "Poison", 40, 65, 95, 60, 45, 35, ownerId, 35, "Levitate")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 35) {
            using (var context = new DatabaseContext())
            {
                var weezing = new Weezing(this);
                weezing.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(weezing);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Koffing to a Weezing!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}