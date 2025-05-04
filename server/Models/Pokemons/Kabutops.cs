namespace PokemonPocket;

public class Kabutops : PokemonMaster
{
    public string? Nickname {get;set;}

    private Kabutops() { } //For EF Core
    public Kabutops(string nickname, string ownerId) 
    : base("Kabutops", "Rock/Water", 60, 115, 105, 65, 70, 80, ownerId, 40, "Swift Swim")
    {
        Nickname = nickname;
    }

    public Kabutops(Kabuto kabuto)
    : base("Kabutops", "Rock/Water", 60, 115, 105, 65, 70, 80, kabuto.OwnerId?? "Unknown", 40, "Swift Swim")
    {
        Id = kabuto.Id;
        Level = 1;
        Nickname = kabuto.Nickname;
        Experience = kabuto.Experience;
        HpIV = kabuto.HpIV;
        AttackIV = kabuto.AttackIV;
        SpecialAttackIV = kabuto.SpecialAttackIV;
        DefenseIV = kabuto.DefenseIV;
        SpecialDefenseIV = kabuto.SpecialDefenseIV;
        SpeedIV = kabuto.SpeedIV;
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