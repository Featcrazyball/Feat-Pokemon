using Database;
namespace PokemonPocket;

public class Omanyte : PokemonMaster
{
    public string? Nickname {get;set;}

    private Omanyte() { } //For EF Core
    public Omanyte(string nickname, string ownerId) 
    : base("Omanyte", "Rock/Water", 35, 40, 100, 90, 55, 35, ownerId, 20, "Swift Swim")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        if (Level >= 40) {
            using (var context = new DatabaseContext())
            {
                var omastar = new Omastar(this);
                omastar.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(omastar);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Omanyte to a Omastar!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}