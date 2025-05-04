using Database;
namespace PokemonPocket;

public class Caterpie : PokemonMaster
{
    public string? Nickname {get;set;}

    private Caterpie() { } //For EF Core
    public Caterpie(string nickname, string ownerId) 
    : base("Caterpie", "Bug", 45, 30, 35, 20, 20, 45, ownerId,  10, "Shield Dust")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 7) {
            using (var context = new DatabaseContext())
            {
                var metapod = new Metapod(this);
                metapod.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(metapod);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Caterpie to a Metapod!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}