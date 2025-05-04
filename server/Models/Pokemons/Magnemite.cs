using Database;
namespace PokemonPocket;

public class Magnemite : PokemonMaster
{
    public string? Nickname {get;set;}

    private Magnemite() { } //For EF Core
    public Magnemite(string nickname, string ownerId) 
    : base("Magnemite", "Electric/Steel", 25, 35, 70, 95, 55, 45, ownerId, 10, "Magnet Pull")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 30) {
            using (var context = new DatabaseContext())
            {
                var magnetron = new Magnetron(this);
                magnetron.EvolveLevelUp(Level-1); 

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(magnetron);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Magnemite to a Magnetron!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}