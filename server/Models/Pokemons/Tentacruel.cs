using Server;
namespace PokemonPocket;

public class Tentacruel : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Tentacruel() { } //For EF Core
    public Tentacruel(string nickname, string ownerId) 
    : base("Tentacruel", "Water/Poison", 80, 70, 65, 80, 120, 100, ownerId, 30, "Liquid Ooze")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public Tentacruel(Tentacool tentacool)
    : base("Tentacruel", "Water/Poison", 80, 70, 65, 80, 120, 100, tentacool.OwnerId ?? "Unknown", 30, "Liquid Ooze")
    {
        Id = tentacool.Id;
        Level = 1;
        Nickname = tentacool.Nickname;
        Experience = tentacool.Experience;
        HpIV = tentacool.HpIV;
        AttackIV = tentacool.AttackIV;
        SpecialAttackIV = tentacool.SpecialAttackIV;
        DefenseIV = tentacool.DefenseIV;
        SpecialDefenseIV = tentacool.SpecialDefenseIV;
        SpeedIV = tentacool.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        await session.SendMessageAsync($"{Nickname} is already at its final evolution stage.");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}