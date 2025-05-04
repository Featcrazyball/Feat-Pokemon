using Database;
namespace PokemonPocket;

public class Machoke : PokemonMaster
{
    public string? Nickname {get;set;}

    private Machoke() { } //For EF Core
    public Machoke(string nickname, string ownerId) 
    : base("Machoke", "Fighting", 80, 100, 70, 50, 60, 45, ownerId, 20, "Guts")
    {
        Nickname = nickname;
    }

    public Machoke(Machop machop)
    : base("Machoke", "Fighting", 80, 100, 70, 50, 60, 45, machop.OwnerId ?? "Unknown", 20, "Guts")
    {
        Id = machop.Id;
        Level = 1;
        Nickname = machop.Nickname;
        Experience = machop.Experience;
        HpIV = machop.HpIV;
        AttackIV = machop.AttackIV;
        SpecialAttackIV = machop.SpecialAttackIV;
        DefenseIV = machop.DefenseIV;
        SpecialDefenseIV = machop.SpecialDefenseIV;
        SpeedIV = machop.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
    }

    public override void Evolve()
    {
        if (Level >= 28) {
            using (var context = new DatabaseContext())
            {
                var machamp = new Machamp(this);
                machamp.EvolveLevelUp(Level-1); 

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(machamp);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Machoke to a Machamp!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}