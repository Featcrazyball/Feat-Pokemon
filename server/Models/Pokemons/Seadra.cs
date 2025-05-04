namespace PokemonPocket;

public class Seadra : PokemonMaster
{
    public string? Nickname {get;set;}

    private Seadra() { } //For EF Core
    public Seadra(string nickname, string ownerId) 
    : base("Seadra", "Water", 55, 65, 95, 95, 45, 85, ownerId, 25, "Poison Point")
    {
        Nickname = nickname;
    }

    public Seadra(Horsea horsea)
    : base("Seadra", "Water", 55, 65, 95, 95, 45, 85, horsea.OwnerId ?? "Unknown", 25, "Poison Point")
    {
        Id = horsea.Id;
        Level = 1;
        Nickname = horsea.Nickname;
        Experience = horsea.Experience;
        HpIV = horsea.HpIV;
        AttackIV = horsea.AttackIV;
        SpecialAttackIV = horsea.SpecialAttackIV;
        DefenseIV = horsea.DefenseIV;
        SpecialDefenseIV = horsea.SpecialDefenseIV;
        SpeedIV = horsea.SpeedIV;
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