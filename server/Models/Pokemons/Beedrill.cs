namespace PokemonPocket;

public class Beedrill : PokemonMaster
{
    public string? Nickname {get;set;}

    private Beedrill() { } //For EF Core
    public Beedrill(string nickname, string ownerId) 
    : base("Beedrill", "Bug/Poison", 65, 90, 40, 45, 80, 75, ownerId, 20, "Swarm")
    {
        Nickname = nickname;
    }

    public Beedrill(Kakuna kakuna)
    : base("Beedrill", "Bug/Poison", 65, 90, 40, 45, 80, 75, kakuna.OwnerId ?? "Unknown", 20, "Swarm")
    {
        Id = kakuna.Id;
        Level = 1;
        Nickname = kakuna.Nickname;
        Experience = kakuna.Experience;
        HpIV = kakuna.HpIV;
        AttackIV = kakuna.AttackIV;
        SpecialAttackIV = kakuna.SpecialAttackIV;
        DefenseIV = kakuna.DefenseIV;
        SpecialDefenseIV = kakuna.SpecialDefenseIV;
        SpeedIV = kakuna.SpeedIV;
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