using Database;
namespace PokemonPocket;

public class Wartortle : PokemonMaster
{
    public string? Nickname {get;set;}

    private Wartortle() { } //For EF Core
    public Wartortle(string nickname, string ownerId) 
    : base("Wartortle", "Water", 59, 63, 80, 65, 80, 58, ownerId, 25, "Water Gun")
    {
        Nickname = nickname;
    }

    public Wartortle(Squirtle squirtle)
    : base("Wartortle", "Water", 59, 63, 80, 65, 80, 58, squirtle.OwnerId ?? "Unknown", 25, "Water Gun")
    {
        Id = squirtle.Id;
        Level = 1;
        Nickname = squirtle.Nickname;
        Experience = squirtle.Experience;
        HpIV = squirtle.HpIV;
        AttackIV = squirtle.AttackIV;
        SpecialAttackIV = squirtle.SpecialAttackIV;
        DefenseIV = squirtle.DefenseIV;
        SpecialDefenseIV = squirtle.SpecialDefenseIV;
        SpeedIV = squirtle.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
    }

    public override void Evolve()
    {
        if (Level >= 36) {
            using (var context = new DatabaseContext())
            {
                var blastoise = new Blastoise(this);
                blastoise.EvolveLevelUp(Level-1); // Level up to 36

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(blastoise);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Wartortle to a Blastoise!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}
