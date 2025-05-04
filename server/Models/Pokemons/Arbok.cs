namespace PokemonPocket;

public class Arbok : PokemonMaster
{
    public string? Nickname {get;set;}

    private Arbok() { } //For EF Core
    public Arbok(string nickname, string ownerId) 
    : base("Arbok", "Poison", 60, 95, 69, 65, 79, 80, ownerId, 25, "Bite")
    {
        Nickname = nickname;
    }

    public Arbok(Ekans ekans)
    : base("Arbok", "Poison", 60, 95, 69, 65, 79, 80, ekans.OwnerId ?? "Unknown", 25, "Bite")
    {
        Id = ekans.Id;
        Level = 1;
        Nickname = ekans.Nickname;
        Experience = ekans.Experience;
        HpIV = ekans.HpIV;
        AttackIV = ekans.AttackIV;
        SpecialAttackIV = ekans.SpecialAttackIV;
        DefenseIV = ekans.DefenseIV;
        SpecialDefenseIV = ekans.SpecialDefenseIV;
        SpeedIV = ekans.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
    }

    public override void Evolve()
    {
        Console.WriteLine($"{Nickname} is already at its final form!");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}