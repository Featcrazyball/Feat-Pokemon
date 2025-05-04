namespace PokemonPocket;

public class Vaporeon : PokemonMaster
{
    public string? Nickname {get;set;}

    private Vaporeon() { } //For EF Core
    public Vaporeon(string nickname, string ownerId) 
    : base("Vaporeon", "Water", 130, 65, 60, 110, 95, 65, ownerId, 30, "Water Absorb")
    {
        Nickname = nickname;
    }

    public Vaporeon(Eevee eevee)
    : base("Vaporeon", "Water", 130, 65, 60, 110, 95, 65, eevee.OwnerId?? "Unknown", 30, "Water Absorb")
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