using Database;
namespace PokemonPocket;

public class Paras : PokemonMaster
{
    public string? Nickname {get;set;}

    private Paras() { } //For EF Core
    public Paras(string nickname, string ownerId) 
    : base("Paras", "Bug/Grass", 35, 70, 55, 45, 55, 25, ownerId, 12, "Effect Spore")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 24) {
            using (var context = new DatabaseContext())
            {
                var parasect = new Parasect(this);
                parasect.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(parasect);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Paras to a Parasect!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}