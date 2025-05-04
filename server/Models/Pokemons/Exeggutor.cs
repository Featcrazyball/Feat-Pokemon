namespace PokemonPocket;

public class Exeggutor : PokemonMaster
{
    public string? Nickname {get;set;}

    private Exeggutor() { } //For EF Core
    public Exeggutor(string nickname, string ownerId) 
    : base("Exeggutor", "Grass/Psychic", 95, 95, 85, 125, 75, 55, ownerId, 30, "Chlorophyll")
    {
        Nickname = nickname;
    }

    public Exeggutor(Exeggcute exeggcute)
    : base("Exeggcute", "Grass/Psychic", 95, 95, 85, 125, 75, 55, exeggcute.OwnerId ?? "Unknown", 30, "Chlorophyll")
    {
        Id = exeggcute.Id;
        Level = 1;
        Nickname = exeggcute.Nickname;
        Experience = exeggcute.Experience;
        HpIV = exeggcute.HpIV;
        AttackIV = exeggcute.AttackIV;
        SpecialAttackIV = exeggcute.SpecialAttackIV;
        DefenseIV = exeggcute.DefenseIV;
        SpecialDefenseIV = exeggcute.SpecialDefenseIV;
        SpeedIV = exeggcute.SpeedIV;
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