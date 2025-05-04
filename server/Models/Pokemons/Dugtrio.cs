namespace PokemonPocket;

public class Dugtrio : PokemonMaster
{
    public string? Nickname {get;set;}

    private Dugtrio() { } //For EF Core
    public Dugtrio(string nickname, string ownerId) 
    : base("Dugtrio", "Ground", 35, 100, 50, 50, 70, 120, ownerId, 26, "Sand Veil")
    {
        Nickname = nickname;
    }

    public Dugtrio(Diglett diglett)
    : base("Dugtrio", "Ground", 35, 100, 50, 50, 70, 120, diglett.OwnerId ?? "Unknown", 26, "Sand Veil")
    {
        Id = diglett.Id;
        Level = 1;
        Nickname = diglett.Nickname;
        Experience = diglett.Experience;
        HpIV = diglett.HpIV;
        AttackIV = diglett.AttackIV;
        SpecialAttackIV = diglett.SpecialAttackIV;
        DefenseIV = diglett.DefenseIV;
        SpecialDefenseIV = diglett.SpecialDefenseIV;
        SpeedIV = diglett.SpeedIV;
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