using Database;
namespace PokemonPocket;

public class Poliwhirl : PokemonMaster
{
    public string? Nickname {get;set;}

    private Poliwhirl() { } //For EF Core
    public Poliwhirl(string nickname, string ownerId) 
    : base("Poliwhirl", "Water", 65, 65, 65, 50, 50, 90, ownerId, 25, "Water Absorb")
    {
        Nickname = nickname;
    }
    
    public Poliwhirl(Poliwag poliwag)
    : base("Poliwhirl", "Water", 65, 65, 65, 50, 50, 90, poliwag.OwnerId ?? "Unknown", 25, "Water Absorb")
    {
        Id = poliwag.Id;
        Level = 1;
        Nickname = poliwag.Nickname;
        Experience = poliwag.Experience;
        HpIV = poliwag.HpIV;
        AttackIV = poliwag.AttackIV;
        SpecialAttackIV = poliwag.SpecialAttackIV;
        DefenseIV = poliwag.DefenseIV;
        SpecialDefenseIV = poliwag.SpecialDefenseIV;
        SpeedIV = poliwag.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
    }

    public override void Evolve()
    {
        using (var context = new DatabaseContext())
        {
            var item = context.Items.FirstOrDefault(i => i.Name == "Water Stone" && i.OwnerId == OwnerId);
            if (item != null) {
                context.Items.Remove(item);
            } else {
                Console.WriteLine($"{Nickname} needs a Water Stone to evolve!");
                return;
            }

            var poliwrath = new Poliwrath(this);
            poliwrath.EvolveLevelUp(Level-1); // Level up to current level

            // Remove previous and add new Pokemon
            context.PokemonMaster.Add(poliwrath);
            context.PokemonMaster.Remove(this);
            context.SaveChanges();
        }
        Console.WriteLine($"{Nickname} has evolved from a Poliwhirl to a Poliwrath!");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}