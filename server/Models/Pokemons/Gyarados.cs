namespace PokemonPocket;

public class Gyarados : PokemonMaster
{
    public string? Nickname {get;set;}

    private Gyarados() { } //For EF Core
    public Gyarados(string nickname, string ownerId) 
    : base("Gyarados", "Water/Ice", 95, 125, 79, 60, 100, 81, ownerId, 30, "Intimidate")
    {
        Nickname = nickname;
    }

    public Gyarados(Magikarp magikarp)
    : base("Gyarados", "Water/Ice", 95, 125, 79, 60, 100, 81, magikarp.OwnerId?? "Unknown", 30, "Intimidate")
    {
        Id = magikarp.Id;
        Level = 1;
        Nickname = magikarp.Nickname;
        Experience = magikarp.Experience;
        HpIV = magikarp.HpIV;
        AttackIV = magikarp.AttackIV;
        SpecialAttackIV = magikarp.SpecialAttackIV;
        DefenseIV = magikarp.DefenseIV;
        SpecialDefenseIV = magikarp.SpecialDefenseIV;
        SpeedIV = magikarp.SpeedIV;
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