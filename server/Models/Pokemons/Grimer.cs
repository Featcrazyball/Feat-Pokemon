using Database;
namespace PokemonPocket;

public class Grimer : PokemonMaster
{
    public string? Nickname {get;set;}

    private Grimer() { } //For EF Core
    public Grimer(string nickname, string ownerId) 
    : base("Grimer", "Poison", 80, 80, 50, 40, 50, 25, ownerId, 15, "Poison Touch")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 38) {
            using (var context = new DatabaseContext())
            {
                var muk = new Muk(this);
                muk.EvolveLevelUp(Level-1); 

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(muk);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Grimer to a Muk!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}