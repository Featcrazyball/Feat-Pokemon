namespace PokemonPocket;

public class Nidoking : PokemonMaster
{
    public string? Nickname {get;set;}

    private Nidoking() { } //For EF Core
    public Nidoking(string nickname, string ownerId)
    : base("Nidoking", "Poison/Ground", 81, 102, 77, 85, 75, 85, ownerId, 30, "Poison Point")
    {
        Nickname = nickname;
    }

    public Nidoking(Nidorino nidorino)
    : base("Nidoking", "Poison/Ground", 81, 102, 77, 85, 75, 85, nidorino.OwnerId ?? "Unknown", 30, "Poison Point")
    {
        Id = nidorino.Id;
        Level = 1;
        Nickname = nidorino.Nickname;
        Experience = nidorino.Experience;
        HpIV = nidorino.HpIV;
        AttackIV = nidorino.AttackIV;
        SpecialAttackIV = nidorino.SpecialAttackIV;
        DefenseIV = nidorino.DefenseIV;
        SpecialDefenseIV = nidorino.SpecialDefenseIV;
        SpeedIV = nidorino.SpeedIV;
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