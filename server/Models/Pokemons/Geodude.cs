using Database;
namespace PokemonPocket;

public class Geodude : PokemonMaster
{
    public string? Nickname {get;set;}

    private Geodude() { } //For EF Core
    public Geodude(string nickname, string ownerId) 
    : base("Geodude", "Rock/Ground", 40, 80, 100, 30, 30, 20, ownerId, 10, "Sturdy")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 25) {
            using (var context = new DatabaseContext())
            {
                var graveler = new Graveler(this);
                graveler.EvolveLevelUp(Level-1); 

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(graveler);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Geodude to a Graveler!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}