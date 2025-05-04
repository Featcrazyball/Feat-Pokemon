namespace PokemonPocket;

public class Blastoise : PokemonMaster
{
    public string? Nickname {get;set;}

    private Blastoise() { } //For EF Core
    public Blastoise(string nickname, string ownerId) 
    : base("Blastoise", "Water", 79, 83, 100, 85, 105, 78, ownerId, 30, "Torrent")
    {
        Nickname = nickname;
    }

    public Blastoise(Wartortle wartortle)
    : base("Blastoise", "Water", 79, 83, 100, 85, 105, 78, wartortle.OwnerId ?? "Unknown", 30, "Torrent")
    {
        Id = wartortle.Id;
        Level = 1;
        Nickname = wartortle.Nickname;
        Experience = wartortle.Experience;
        HpIV = wartortle.HpIV;
        AttackIV = wartortle.AttackIV;
        SpecialAttackIV = wartortle.SpecialAttackIV;
        DefenseIV = wartortle.DefenseIV;
        SpecialDefenseIV = wartortle.SpecialDefenseIV;
        SpeedIV = wartortle.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
    }

    public override void Evolve()
    {
        Console.WriteLine($"{Nickname} is already at its final evolution stage.");
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}