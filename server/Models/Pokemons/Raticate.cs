namespace PokemonPocket;

public class Raticate : PokemonMaster
{
    public string? Nickname {get;set;}

    private Raticate() { } //For EF Core
    public Raticate(string nickname, string ownerId) 
    : base("Raticate", "Normal", 55, 81, 60, 50, 70, 97, ownerId, 25, "Run Away")
    {
        Nickname = nickname;
    }

    public Raticate(Rattata rattata)
    : base("Raticate", "Normal", 55, 81, 60, 50, 70, 97, rattata.OwnerId ?? "Unknown", 25, "Run Away")
    {
        Id = rattata.Id;
        Level = 1;
        Nickname = rattata.Nickname;
        Experience = rattata.Experience;
        HpIV = rattata.HpIV;
        AttackIV = rattata.AttackIV;
        SpecialAttackIV = rattata.SpecialAttackIV;
        DefenseIV = rattata.DefenseIV;
        SpecialDefenseIV = rattata.SpecialDefenseIV;
        SpeedIV = rattata.SpeedIV;
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