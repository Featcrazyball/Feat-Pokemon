namespace PokemonPocket;

public class Primeape : PokemonMaster
{
    public string? Nickname {get;set;}

    private Primeape() { } //For EF Core
    public Primeape(string nickname, string ownerId) 
    : base("Primeape", "Fighting", 65, 105, 60, 60, 70, 95, ownerId, 27, "Vital Spirit")
    {
        Nickname = nickname;
    }

    public Primeape(Mankey mankey)
    : base("Primeape", "Fighting", 65, 105, 60, 60, 70, 95, mankey.OwnerId ?? "Unknown", 27, "Vital Spirit")
    {
        Id = mankey.Id;
        Level = 1;
        Nickname = mankey.Nickname;
        Experience = mankey.Experience; 
        HpIV = mankey.HpIV;
        AttackIV = mankey.AttackIV;
        SpecialAttackIV = mankey.SpecialAttackIV;
        DefenseIV = mankey.DefenseIV;
        SpecialDefenseIV = mankey.SpecialDefenseIV;
        SpeedIV = mankey.SpeedIV;
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