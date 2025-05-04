using Database;
namespace PokemonPocket;

public class Charmeleon : PokemonMaster
{
    public string? Nickname {get;set;}

    private Charmeleon() { } //For EF Core
    public Charmeleon(string nickname, string ownerId) 
    : base("Charmeleon", "Fire", 58, 64, 58, 80, 65, 80, ownerId, 25, "Fire Burst")
    {
        Nickname = nickname;
    }

    public Charmeleon(Charmander charm)
    : base("Charmeleon", "Fire", 58, 64, 58, 80, 65, 80, charm.OwnerId ?? "Unknown", 25, "Fire Burst")
    {
        Id = charm.Id;
        Level = 1;
        Nickname = charm.Nickname;
        Experience = charm.Experience;
        HpIV = charm.HpIV;
        AttackIV = charm.AttackIV;
        SpecialAttackIV = charm.SpecialAttackIV;
        DefenseIV = charm.DefenseIV;
        SpecialDefenseIV = charm.SpecialDefenseIV;
        SpeedIV = charm.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
    }

    public override void Evolve()
    {
        if (Level >= 36) {
            using (var context = new DatabaseContext())
            {
                var charizard = new Charizard(this);
                charizard.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(charizard);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Charmeleon to a Charizard!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}