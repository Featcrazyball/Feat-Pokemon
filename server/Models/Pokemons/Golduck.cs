namespace PokemonPocket;

public class Golduck : PokemonMaster
{
    public string? Nickname {get;set;}

    private Golduck() { } //For EF Core
    public Golduck(string nickname, string ownerId) 
    : base("Golduck", "Water", 80, 82, 78, 95, 80, 85, ownerId, 55, "Damp")
    {
        Nickname = nickname;
    }

    public Golduck(Psyduck psyduck)
    : base("Venomoth", "Bug/Poison", 80, 82, 78, 95, 80, 85, psyduck.OwnerId ?? "Unknown", 55, "Damp")
    {
        Id = psyduck.Id;
        Level = 1;
        Nickname = psyduck.Nickname;
        Experience = psyduck.Experience;
        HpIV = psyduck.HpIV;
        AttackIV = psyduck.AttackIV;
        SpecialAttackIV = psyduck.SpecialAttackIV;
        DefenseIV = psyduck.DefenseIV;
        SpecialDefenseIV = psyduck.SpecialDefenseIV;
        SpeedIV = psyduck.SpeedIV;
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