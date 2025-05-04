namespace PokemonPocket;

public class Cloyster : PokemonMaster
{
    public string? Nickname {get;set;}

    private Cloyster() { } //For EF Core
    public Cloyster(string nickname, string ownerId) 
    : base("Cloyster", "Water/Ice", 50, 95, 180, 85, 45, 70, ownerId, 30, "Shell Armor")
    {
        Nickname = nickname;
    }

    public Cloyster(Shellder shellder)
    : base("Cloyster", "Water/Ice", 50, 95, 180, 85, 45, 70, shellder.OwnerId ?? "Unknown", 30, "Shell Armor")
    {
        Id = shellder.Id;
        Level = 1;
        Nickname = shellder.Nickname;
        Experience = shellder.Experience;
        HpIV = shellder.HpIV;
        AttackIV = shellder.AttackIV;
        SpecialAttackIV = shellder.SpecialAttackIV;
        DefenseIV = shellder.DefenseIV;
        SpecialDefenseIV = shellder.SpecialDefenseIV;
        SpeedIV = shellder.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
    }

    public override void Evolve()
    {
        Console.WriteLine($"{Nickname} is already at its final evolution stage.");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}