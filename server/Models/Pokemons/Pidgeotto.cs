using Database;
namespace PokemonPocket;

public class Pidgeotto : PokemonMaster
{
    public string? Nickname {get;set;}

    private Pidgeotto() { } //For EF Core
    public Pidgeotto(string nickname, string ownerId) 
    : base("Pidgeotto", "Normal/Flying", 63, 60, 55, 50, 50, 71, ownerId, 25, "Gust")
    {
        Nickname = nickname;
    }

    public Pidgeotto(Pidgey pidgey)
    : base("Pidgeotto", "Normal/Flying", 63, 60, 55, 50, 50, 71, pidgey.OwnerId ?? "Unknown", 25, "Gust")
    {
        Id = pidgey.Id;
        Level = 1;
        Nickname = pidgey.Nickname;
        Experience = pidgey.Experience;
        HpIV = pidgey.HpIV;
        AttackIV = pidgey.AttackIV;
        SpecialAttackIV = pidgey.SpecialAttackIV;
        DefenseIV = pidgey.DefenseIV;
        SpecialDefenseIV = pidgey.SpecialDefenseIV;
        SpeedIV = pidgey.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
    }

    public override void Evolve()
    {
        if (Level >= 36) {
            using (var context = new DatabaseContext())
            {
                var pidgeot = new Pidgeot(this);
                pidgeot.EvolveLevelUp(Level-1); // Level up to 36

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(pidgeot);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Pidgeotto to a Pidgeot!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}