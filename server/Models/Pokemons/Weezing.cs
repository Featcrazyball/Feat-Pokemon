namespace PokemonPocket;

public class Weezing : PokemonMaster
{
    public string? Nickname {get;set;}

    private Weezing() { } //For EF Core
    public Weezing(string nickname, string ownerId) 
    : base("Weezing", "Poison", 65, 90, 120, 85, 70, 60, ownerId, 35, "Levitate")
    {
        Nickname = nickname;
    }
    
    public Weezing(Koffing koffing)
    : base("Weezing", "Poison", 65, 90, 120, 85, 70, 60, koffing.OwnerId ?? "Unknown", 35, "Levitate")
    {
        Id = koffing.Id;
        Level = 1;
        Nickname = koffing.Nickname;
        Experience = koffing.Experience;
        HpIV = koffing.HpIV;
        AttackIV = koffing.AttackIV;
        SpecialAttackIV = koffing.SpecialAttackIV;
        DefenseIV = koffing.DefenseIV;
        SpecialDefenseIV = koffing.SpecialDefenseIV;
        SpeedIV = koffing.SpeedIV;
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