namespace PokemonPocket;

public class Venusaur : PokemonMaster
{
    public string? Nickname {get;set;}

    private Venusaur() { } //For EF Core
    public Venusaur(string nickname, string ownerId) 
    : base("Venusaur", "Grass/Poison", 80, 82, 83, 100, 100, 80, ownerId, 30, "Water Burst")
    {
        Nickname = nickname;
    }

    public Venusaur(Ivysaur ivysaur)
    : base("Venusaur", "Grass/Poison", 80, 82, 83, 100, 100, 80, ivysaur.OwnerId ?? "Unknown", 30, "Water Burst")
    {
        Id = ivysaur.Id;
        Level = 1;
        Nickname = ivysaur.Nickname;
        Experience = ivysaur.Experience;
        HpIV = ivysaur.HpIV;
        AttackIV = ivysaur.AttackIV;
        SpecialAttackIV = ivysaur.SpecialAttackIV;
        DefenseIV = ivysaur.DefenseIV;
        SpecialDefenseIV = ivysaur.SpecialDefenseIV;
        SpeedIV = ivysaur.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
    }

    public override void Evolve()
    {
        Console.WriteLine($"{Nickname} is already at its final form!");
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}