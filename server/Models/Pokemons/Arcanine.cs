namespace PokemonPocket;

public class Arcanine : PokemonMaster
{
    public string? Nickname {get;set;}

    private Arcanine() { } //For EF Core
    public Arcanine(string nickname, string ownerId) 
    : base("Arcanine", "Fire", 90, 110, 80, 100, 80, 95, ownerId, 59, "Intimidate")
    {
        Nickname = nickname;
    }

    public Arcanine(Growlithe growlithe)
    : base("Arcanine", "Fire", 90, 110, 80, 100, 80, 95, growlithe.OwnerId ?? "Unknown", 59, "Intimidate")
    {
        Id = growlithe.Id;
        Level = 1;
        Nickname = growlithe.Nickname;
        Experience = growlithe.Experience;
        HpIV = growlithe.HpIV;
        AttackIV = growlithe.AttackIV;
        SpecialAttackIV = growlithe.SpecialAttackIV;
        DefenseIV = growlithe.DefenseIV;
        SpecialDefenseIV = growlithe.SpecialDefenseIV;
        SpeedIV = growlithe.SpeedIV;
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