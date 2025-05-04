namespace PokemonPocket;

public class Parasect : PokemonMaster
{
    public string? Nickname {get;set;}

    private Parasect() { } //For EF Core
    public Parasect(string nickname, string ownerId) 
    : base("Parasect", "Bug/Grass", 60, 95, 80, 60, 80, 30, ownerId, 24, "Effect Spore")
    {
        Nickname = nickname;
    }
    
    public Parasect(Paras paras)
    : base("Parasect", "Bug/Grass", 60, 95, 80, 60, 80, 30, paras.OwnerId ?? "Unknown", 24, "Effect Spore")
    {
        Id = paras.Id;
        Level = 1;
        Nickname = paras.Nickname;
        Experience = paras.Experience;
        HpIV = paras.HpIV;
        AttackIV = paras.AttackIV;
        SpecialAttackIV = paras.SpecialAttackIV;
        DefenseIV = paras.DefenseIV;
        SpecialDefenseIV = paras.SpecialDefenseIV;
        SpeedIV = paras.SpeedIV;
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