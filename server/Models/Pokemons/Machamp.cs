using Server;
namespace PokemonPocket;

public class Machamp : PokemonMaster
{
    private Machamp() { } //For EF Core
    public Machamp(string nickname, string ownerId) 
    : base("Machamp", "Fighting", 90, 130, 80, 65, 85, 55, ownerId, 20, "No Guard")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public Machamp(Machoke machoke)
    : base("Machamp", "Fighting", 90, 130, 80, 65, 85, 55, machoke.OwnerId ?? "Unknown", 20, "No Guard")
    {
        Id = machoke.Id;
        Level = 1;
        Nickname = machoke.Nickname;
        Experience = machoke.Experience;
        HpIV = machoke.HpIV;
        AttackIV = machoke.AttackIV;
        SpecialAttackIV = machoke.SpecialAttackIV;
        DefenseIV = machoke.DefenseIV;
        SpecialDefenseIV = machoke.SpecialDefenseIV;
        SpeedIV = machoke.SpeedIV;
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