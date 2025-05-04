using Database;
namespace PokemonPocket;

public class Metapod : PokemonMaster
{
    public string? Nickname {get;set;}

    private Metapod() { } //For EF Core
    public Metapod(string nickname, string ownerId) 
    : base("Metapod", "Bug", 50, 20, 55, 25, 25, 30, ownerId, 25, "Harden")
    {
        Nickname = nickname;
    }

    public Metapod(Caterpie caterpie)
    : base("Metapod", "Bug", 50, 20, 55, 25, 25, 30, caterpie.OwnerId ?? "Unknown", 25, "Harden")
    {
        Id = caterpie.Id;
        Level = 1;
        Nickname = caterpie.Nickname;
        Experience = caterpie.Experience;
        HpIV = caterpie.HpIV;
        AttackIV = caterpie.AttackIV;
        SpecialAttackIV = caterpie.SpecialAttackIV;
        DefenseIV = caterpie.DefenseIV;
        SpecialDefenseIV = caterpie.SpecialDefenseIV;
        SpeedIV = caterpie.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
    }

    public override void Evolve()
    {
        if (Level >= 10) {
            using (var context = new DatabaseContext())
            {
                var butterfree = new Butterfree(this);
                butterfree.EvolveLevelUp(Level-1); // Level up to 10

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(butterfree);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Metapod to a Butterfree!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}