namespace PokemonPocket;

public class Venomoth : PokemonMaster
{
    public string? Nickname {get;set;}

    private Venomoth() { } //For EF Core
    public Venomoth(string nickname, string ownerId) 
    : base("Venomoth", "Bug/Poison", 70, 65, 60, 90, 75, 90, ownerId, 31, "Shield Dust")
    {
        Nickname = nickname;
    }

    public Venomoth(Venonat venonat)
    : base("Venomoth", "Bug/Poison", 70, 65, 60, 90, 75, 90, venonat.OwnerId ?? "Unknown", 31, "Shield Dust")
    {
        Id = venonat.Id;
        Level = 1;
        Nickname = venonat.Nickname;
        Experience = venonat.Experience;
        HpIV = venonat.HpIV;
        AttackIV = venonat.AttackIV;
        SpecialAttackIV = venonat.SpecialAttackIV;
        DefenseIV = venonat.DefenseIV;
        SpecialDefenseIV = venonat.SpecialDefenseIV;
        SpeedIV = venonat.SpeedIV;
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