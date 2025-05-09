using Server;
namespace PokemonPocket;

public class Muk : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Muk() { } //For EF Core
    public Muk(string nickname, string ownerId) 
    : base("Muk", "Poison", 105, 105, 75, 65, 100, 50, ownerId, 35, "Poison Touch")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public Muk(Grimer grimer)
    : base("Muk", "Poison", 105, 105, 75, 65, 100, 50, grimer.OwnerId ?? "Unknown", 35, "Poison Touch")
    {
        Id = grimer.Id;
        Level = 1;
        Nickname = grimer.Nickname;
        Experience = grimer.Experience;
        HpIV = grimer.HpIV;
        AttackIV = grimer.AttackIV;
        SpecialAttackIV = grimer.SpecialAttackIV;
        DefenseIV = grimer.DefenseIV;
        SpecialDefenseIV = grimer.SpecialDefenseIV;
        SpeedIV = grimer.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
    }

    public override async Task Evolve(ClientSession session)
    {
        await session.SendMessageAsync($"{Nickname} is already at its final evolution stage.");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}