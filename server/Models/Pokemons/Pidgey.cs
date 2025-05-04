using Database;
namespace PokemonPocket;

public class Pidgey : PokemonMaster
{
    public string? Nickname {get;set;}

    private Pidgey() { } //For EF Core
    public Pidgey(string nickname, string ownerId) 
    : base("Pidgey", "Normal/Flying", 40, 45, 40, 35, 35, 56, ownerId, 10, "Keen Eye")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 18) {
            using (var context = new DatabaseContext())
            {
                var pidgeotto = new Pidgeotto(this);
                pidgeotto.EvolveLevelUp(Level-1); // Level up to 18

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(pidgeotto);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Pidgey to a Pidgeotto!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}