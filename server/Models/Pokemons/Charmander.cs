using Database;
namespace PokemonPocket;
    
public class Charmander : PokemonMaster
{
    public string? Nickname {get;set;}

    private Charmander() { } //For EF Core
    public Charmander(string nickname, string ownerId) 
    : base("Charmander", "Fire", 39, 52, 43, 60, 50, 65, ownerId, 10, "Solar Power")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 16) {
            using (var context = new DatabaseContext())
            {
                var charmeleon = new Charmeleon(this);
                charmeleon.EvolveLevelUp(Level-1); 

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(charmeleon);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Charmander to a Charmeleon!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}