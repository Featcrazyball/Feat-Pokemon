namespace PokemonPocket;

public class Poliwrath : PokemonMaster
{
    public string? Nickname {get;set;}

    private Poliwrath() { } //For EF Core
    public Poliwrath(string nickname, string ownerId) 
    : base("Poliwrath", "Water", 90, 95, 95, 70, 90, 70, ownerId, 60, "Water Absorb")
    {
        Nickname = nickname;
    }

    public Poliwrath(Poliwhirl poliwhirl)
    : base("Poliwrath", "Water", 90, 95, 95, 70, 90, 70, poliwhirl.OwnerId ?? "Unknown", 60, "Water Absorb")
    {
        Id = poliwhirl.Id;
        Level = 1;
        Nickname = poliwhirl.Nickname;
        Experience = poliwhirl.Experience;
        HpIV = poliwhirl.HpIV;
        AttackIV = poliwhirl.AttackIV;
        SpecialAttackIV = poliwhirl.SpecialAttackIV;
        DefenseIV = poliwhirl.DefenseIV;
        SpecialDefenseIV = poliwhirl.SpecialDefenseIV;
        SpeedIV = poliwhirl.SpeedIV;
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