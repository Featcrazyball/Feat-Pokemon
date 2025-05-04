namespace PokemonPocket;

public class Wigglytuff : PokemonMaster
{
    public string? Nickname {get;set;}

    private Wigglytuff() { } //For EF Core
    public Wigglytuff(string nickname, string ownerId) 
    : base("Wigglytuff", "Normal/Fairy", 140, 70, 45, 85, 50, 45, ownerId, 30, "Cute Charm")
    {
        Nickname = nickname;
    }

    public Wigglytuff(Jigglypuff jigglypuff)
    : base("Wigglytuff", "Normal/Fairy", 140, 70, 45, 85, 50, 45, jigglypuff.OwnerId ?? "Unknown", 30, "Cute Charm")
    {
        Id = jigglypuff.Id;
        Level = 1;
        Nickname = jigglypuff.Nickname;
        Experience = jigglypuff.Experience;
        HpIV = jigglypuff.HpIV;
        AttackIV = jigglypuff.AttackIV;
        SpecialAttackIV = jigglypuff.SpecialAttackIV;
        DefenseIV = jigglypuff.DefenseIV;
        SpecialDefenseIV = jigglypuff.SpecialDefenseIV;
        SpeedIV = jigglypuff.SpeedIV;
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