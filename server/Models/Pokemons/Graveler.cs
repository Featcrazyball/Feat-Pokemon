using Database;
namespace PokemonPocket;

public class Graveler : PokemonMaster
{
    public string? Nickname {get;set;}

    private Graveler() { } //For EF Core
    public Graveler(string nickname, string ownerId) 
    : base("Graveler", "Rock/Ground", 55, 95, 115, 45, 45, 35, ownerId, 25, "Sturdy")
    {
        Nickname = nickname;
    }

    public Graveler(Geodude geodude)
    : base("Graveler", "Rock/Ground", 55, 95, 115, 45, 45, 35, geodude.OwnerId ?? "Unknown", 25, "Sturdy")
    {
        Id = geodude.Id;
        Level = 1;
        Nickname = geodude.Nickname;
        Experience = geodude.Experience;
        HpIV = geodude.HpIV;
        AttackIV = geodude.AttackIV;
        SpecialAttackIV = geodude.SpecialAttackIV;
        DefenseIV = geodude.DefenseIV;
        SpecialDefenseIV = geodude.SpecialDefenseIV;
        SpeedIV = geodude.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
    }

    public override void Evolve()
    {
        if (Level >= 1) {
            using (var context = new DatabaseContext())
            {
                var golem = new Golem(this);
                golem.EvolveLevelUp(Level-1); 

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(golem);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Geodude to a Graveler!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}