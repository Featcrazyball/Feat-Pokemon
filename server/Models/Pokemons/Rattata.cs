using Database;
namespace PokemonPocket;

public class Rattata : PokemonMaster
{
    public string? Nickname {get;set;}

    private Rattata() { } //For EF Core
    public Rattata(string nickname, string ownerId) 
    : base("Rattata", "Normal", 30, 56, 35, 25, 35, 72, ownerId, 25, "Run Away")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 20) {
            using (var context = new DatabaseContext())
            {
                var ratticate = new Raticate(this);
                ratticate.EvolveLevelUp(Level-1); // Level up to 20

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(ratticate);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Rattata to a Raticate!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}