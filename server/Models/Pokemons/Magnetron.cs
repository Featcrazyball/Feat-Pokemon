namespace PokemonPocket;

public class Magnetron : PokemonMaster
{
    public string? Nickname {get;set;}

    private Magnetron() { } //For EF Core
    public Magnetron(string nickname, string ownerId) 
    : base("Magnetron", "Electric/Steel", 50, 60, 95, 120, 70, 70, ownerId, 20, "Magnet Pull")
    {
        Nickname = nickname;
    }

    public Magnetron(Magnemite magnemite)
    : base("Magnetron", "Electric/Steel", 50, 60, 95, 120, 70, 70, magnemite.OwnerId ?? "Unknown", 20, "Magnet Pull")
    {
        Id = magnemite.Id;
        Level = 1;
        Nickname = magnemite.Nickname;
        Experience = magnemite.Experience;
        HpIV = magnemite.HpIV;
        AttackIV = magnemite.AttackIV;
        SpecialAttackIV = magnemite.SpecialAttackIV;
        DefenseIV = magnemite.DefenseIV;
        SpecialDefenseIV = magnemite.SpecialDefenseIV;
        SpeedIV = magnemite.SpeedIV;
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