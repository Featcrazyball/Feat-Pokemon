namespace PokemonPocket;

public class Kingler : PokemonMaster
{
    public string? Nickname {get;set;}

    private Kingler() { } //For EF Core
    public Kingler(string nickname, string ownerId) 
    : base("Kingler", "Water", 55, 130, 115, 50, 50, 75, ownerId, 30, "Hyper Cutter")
    {
        Nickname = nickname;
    }

    public Kingler(Krabby krabby)
    : base("Kingler", "Water", 55, 130, 115, 50, 50, 75, krabby.OwnerId ?? "Unknown", 30, "Hyper Cutter")
    {
        Id = krabby.Id;
        Level = 1;
        Nickname = krabby.Nickname;
        Experience = krabby.Experience;
        HpIV = krabby.HpIV;
        AttackIV = krabby.AttackIV;
        SpecialAttackIV = krabby.SpecialAttackIV;
        DefenseIV = krabby.DefenseIV;
        SpecialDefenseIV = krabby.SpecialDefenseIV;
        SpeedIV = krabby.SpeedIV;
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