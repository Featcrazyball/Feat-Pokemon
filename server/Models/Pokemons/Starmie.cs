namespace PokemonPocket;

public class Starmie : PokemonMaster
{
    public string? Nickname {get;set;}

    private Starmie() { } //For EF Core
    public Starmie(string nickname, string ownerId) 
    : base("Starmie", "Water/Psychic", 60, 75, 85, 100, 85, 115, ownerId, 30, "Illuminate")
    {
        Nickname = nickname;
    }

    public Starmie(Staryu staryu)
    : base("Starmie", "Water/Psychic", 60, 75, 85, 100, 85, 115, staryu.OwnerId ?? "Unknown", 30, "Illuminate")
    {
        Id = staryu.Id;
        Level = 1;
        Nickname = staryu.Nickname;
        Experience = staryu.Experience;
        HpIV = staryu.HpIV;
        AttackIV = staryu.AttackIV;
        SpecialAttackIV = staryu.SpecialAttackIV;
        DefenseIV = staryu.DefenseIV;
        SpecialDefenseIV = staryu.SpecialDefenseIV;
        SpeedIV = staryu.SpeedIV;
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