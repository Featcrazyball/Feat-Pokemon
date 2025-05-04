using Database;
namespace PokemonPocket;

public class Butterfree : PokemonMaster
{
    public string? Nickname {get;set;}

    private Butterfree() { } //For EF Core
    public Butterfree(string nickname, string ownerId) 
    : base("Butterfree", "Bug/Flying", 60, 45, 50, 90, 80, 70, ownerId, 25, "Confusion")
    {
        Nickname = nickname;
    }

    public Butterfree(Metapod caterpie)
    : base("Butterfree", "Bug/Flying", 60, 45, 50, 90, 80, 70, caterpie.OwnerId ?? "Unknown", 25, "Confusion")
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
        Console.WriteLine($"{Nickname} is already at its final form!");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}