using Database;
namespace PokemonPocket;

public class Cubone : PokemonMaster
{
    public string? Nickname {get;set;}

    private Cubone() { } //For EF Core
    public Cubone(string nickname, string ownerId) 
    : base("Cubone", "Ground", 50, 50, 95, 40, 50, 35, ownerId, 20, "Lightning Rod")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 28) {
            using (var context = new DatabaseContext())
            {
                var marowak = new Marowak(this);
                marowak.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(marowak);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Cubone to a Marowak!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }


    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}