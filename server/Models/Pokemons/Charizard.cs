namespace PokemonPocket;

public class Charizard : PokemonMaster
{
    public string? Nickname {get;set;}

    private Charizard() { } //For EF Core
    public Charizard(string nickname, string ownerId) 
    : base("Charizard", "Fire/Flying", 78, 84, 78, 109, 85, 100, ownerId, 40, "Fire Burst")
    {
        Nickname = nickname;
    }

    public Charizard(Charmeleon charmander)
    : base("Charizard", "Fire/Flying", 78, 84, 78, 109, 85, 100, charmander.OwnerId ?? "Unknown", 40, "Fire Burst")
    {
        Id = charmander.Id;
        Level = 1;
        Nickname = charmander.Nickname;
        Experience = charmander.Experience;
        HpIV = charmander.HpIV;
        AttackIV = charmander.AttackIV;
        SpecialAttackIV = charmander.SpecialAttackIV;
        DefenseIV = charmander.DefenseIV;
        SpecialDefenseIV = charmander.SpecialDefenseIV;
        SpeedIV = charmander.SpeedIV;
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
