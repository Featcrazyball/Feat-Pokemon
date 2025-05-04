namespace PokemonPocket;

public class Nidoqueen : PokemonMaster
{
    public string? Nickname {get;set;}

    private Nidoqueen() { } //For EF Core
    public Nidoqueen(string nickname, string ownerId)
    : base("Nidoqueen", "Poison/Ground", 90, 82, 87, 75, 85, 76, ownerId, 30, "Poison Point")
    {
        Nickname = nickname;
    }

    public Nidoqueen(Nidorina nidorina)
    : base("Nidoqueen", "Poison/Ground", 90, 82, 87, 75, 85, 76, nidorina.OwnerId ?? "Unknown", 30, "Poison Point")
    {
        Id = nidorina.Id;
        Level = 1;
        Nickname = nidorina.Nickname;
        Experience = nidorina.Experience;
        HpIV = nidorina.HpIV;
        AttackIV = nidorina.AttackIV;
        SpecialAttackIV = nidorina.SpecialAttackIV;
        DefenseIV = nidorina.DefenseIV;
        SpecialDefenseIV = nidorina.SpecialDefenseIV;
        SpeedIV = nidorina.SpeedIV;
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