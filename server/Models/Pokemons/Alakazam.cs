namespace PokemonPocket;

public class Alakazam : PokemonMaster
{
    public string? Nickname {get;set;}

    private Alakazam() { } //For EF Core
    public Alakazam(string nickname, string ownerId) 
    : base("Alakazam", "Psychic", 55, 50, 45, 135, 95, 120, ownerId, 20, "Synchronize")
    {
        Nickname = nickname;
    }

    public Alakazam(Kadabra kadabra) 
    : base("Kadabra", "Psychic", 55, 50, 45, 135, 95, 120, kadabra.OwnerId ?? "Unknown", 50, "Synchronize")
    {
        Id = kadabra.Id;
        Level = 1;
        Nickname = kadabra.Nickname;
        Experience = kadabra.Experience;
        HpIV = kadabra.HpIV;
        AttackIV = kadabra.AttackIV;
        SpecialAttackIV = kadabra.SpecialAttackIV;
        DefenseIV = kadabra.DefenseIV;
        SpecialDefenseIV = kadabra.SpecialDefenseIV;
        SpeedIV = kadabra.SpeedIV;
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