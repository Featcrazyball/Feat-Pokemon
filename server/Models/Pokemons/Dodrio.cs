using Server;
namespace PokemonPocket;

public class Dodrio : PokemonMaster
{
    private Dodrio() { } //For EF Core
    public Dodrio(string nickname, string ownerId) 
    : base("Dodrio", "Normal/Flying", 60, 110, 70, 60, 60, 110, ownerId, 30, "Early Bird")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public Dodrio(Doduo doduo)
    : base("Dodrio", "Normal/Flying", 60, 110, 70, 60, 60, 110, doduo.OwnerId ?? "Unknown", 30, "Early Bird")
    {
        Id = doduo.Id;
        Level = 1;
        Nickname = doduo.Nickname;
        Experience = doduo.Experience;
        HpIV = doduo.HpIV;
        AttackIV = doduo.AttackIV;
        SpecialAttackIV = doduo.SpecialAttackIV;
        DefenseIV = doduo.DefenseIV;
        SpecialDefenseIV = doduo.SpecialDefenseIV;
        SpeedIV = doduo.SpeedIV;
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