namespace PokemonPocket;

public class Jolteon : PokemonMaster
{
    public string? Nickname {get;set;}

    private Jolteon() { } //For EF Core
    public Jolteon(string nickname, string ownerId) 
    : base("Jolteon", "Electric", 65, 65, 60, 110, 95, 130, ownerId, 29, "Volt Absorb")
    {
        Nickname = nickname;
    }

    public Jolteon(Eevee eevee)
    : base("Jolteon", "Electric", 65, 65, 60, 110, 95, 130, eevee.OwnerId?? "Unknown", 29, "Volt Absorb")
    {
        Id = eevee.Id;
        Level = 1;
        Nickname = eevee.Nickname;
        Experience = eevee.Experience;
        HpIV = eevee.HpIV;
        AttackIV = eevee.AttackIV;
        SpecialAttackIV = eevee.SpecialAttackIV;
        DefenseIV = eevee.DefenseIV;
        SpecialDefenseIV = eevee.SpecialDefenseIV;
        SpeedIV = eevee.SpeedIV;
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