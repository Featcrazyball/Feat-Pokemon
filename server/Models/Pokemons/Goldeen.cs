using Database;
namespace PokemonPocket;

public class Goldeen : PokemonMaster
{
    public string? Nickname {get;set;}

    private Goldeen() { } //For EF Core
    public Goldeen(string nickname, string ownerId) 
    : base("Goldeen", "Water", 45, 67, 60, 35, 50, 63, ownerId, 20, "Swift Swim")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 33) {
            using (var context = new DatabaseContext())
            {
                var seaking = new Seaking(this);
                seaking.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(seaking);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Goldeen to a Seaking!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}