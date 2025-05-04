using Database;
namespace PokemonPocket;

public class Diglett : PokemonMaster
{
    public string? Nickname {get;set;}

    private Diglett() { } //For EF Core
    public Diglett(string nickname, string ownerId) 
    : base("Diglett", "Ground", 10, 55, 25, 35, 45, 95, ownerId, 10, "Sand Veil")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 26) {
            using (var context = new DatabaseContext())
            {
                var dugtrio = new Dugtrio(this);
                dugtrio.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(dugtrio);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Diglett to a Dugtrio!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}