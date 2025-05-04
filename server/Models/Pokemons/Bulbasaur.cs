using Database;
namespace PokemonPocket;

public class Bulbasaur : PokemonMaster
{
    public string? Nickname {get;set;}

    private Bulbasaur() { } //For EF Core
    public Bulbasaur(string nickname, string ownerId) 
    : base("Bulbasaur", "Grass/Poison", 45, 49, 49, 65, 65, 45, ownerId, 10, "Water Burst")
    {
        Nickname = nickname;
    }

    // Ask Teacher
    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }

    public override void Evolve()
    {
        if (Level >= 16) {
            using (var context = new DatabaseContext())
            {
                var ivysaur = new Ivysaur(this);
                ivysaur.EvolveLevelUp(Level-1); 

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(ivysaur);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Bulbasaur to a Ivysaur!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }
}