namespace PokemonPocket;

public class Victreebel : PokemonMaster
{
    public string? Nickname {get;set;}

    private Victreebel() { } //For EF Core
    public Victreebel(string nickname, string ownerId) 
    : base("Victreebel", "Grass/Poison", 80, 105, 65, 100, 70, 70, ownerId, 20, "Chlorophyll")
    {
        Nickname = nickname;
    }

    public Victreebel(Weepinbell weepinbell)
    : base("Victreebel", "Grass/Poison", 80, 105, 65, 100, 70, 70, weepinbell.OwnerId ?? "Unknown", 20, "Chlorophyll")
    {
        Id = weepinbell.Id;
        Level = 1;
        Nickname = weepinbell.Nickname;
        Experience = weepinbell.Experience;
        HpIV = weepinbell.HpIV;
        AttackIV = weepinbell.AttackIV;
        SpecialAttackIV = weepinbell.SpecialAttackIV;
        DefenseIV = weepinbell.DefenseIV;
        SpecialDefenseIV = weepinbell.SpecialDefenseIV;
        SpeedIV = weepinbell.SpeedIV;
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