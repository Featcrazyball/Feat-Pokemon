using Database;
namespace PokemonPocket;

public class Bellsprout : PokemonMaster
{
    public string? Nickname {get;set;}

    private Bellsprout() { } //For EF Core
    public Bellsprout(string nickname, string ownerId) 
    : base("Bellsprout", "Grass/Poison", 50, 75, 35, 70, 30, 40, ownerId, 10, "Chlorophyll")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 21) {
            using (var context = new DatabaseContext())
            {
                var weepinbell = new Weepinbell(this);
                weepinbell.EvolveLevelUp(Level-1); 

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(weepinbell);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Bellsprout to a Weepinbell!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}