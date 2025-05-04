namespace PokemonPocket;

public class Omastar : PokemonMaster
{
    public string? Nickname {get;set;}

    private Omastar() { } //For EF Core
    public Omastar(string nickname, string ownerId) 
    : base("Omastar", "Rock/Water", 70, 60, 125, 115, 70, 55, ownerId, 40, "Swift Swim")
    {
        Nickname = nickname;
    }

    public Omastar(Omanyte omanyte)
    : base("Omastar", "Rock/Water", 70, 60, 125, 115, 70, 55, omanyte.OwnerId?? "Unknown", 40, "Swift Swim")
    {
        Id = omanyte.Id;
        Level = 1;
        Nickname = omanyte.Nickname;
        Experience = omanyte.Experience;
        HpIV = omanyte.HpIV;
        AttackIV = omanyte.AttackIV;
        SpecialAttackIV = omanyte.SpecialAttackIV;
        DefenseIV = omanyte.DefenseIV;
        SpecialDefenseIV = omanyte.SpecialDefenseIV;
        SpeedIV = omanyte.SpeedIV;
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