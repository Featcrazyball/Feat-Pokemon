using Server;
namespace PokemonPocket;

public class Dewgong : PokemonMaster
{
    private Dewgong() { } //For EF Core
    public Dewgong(string nickname, string ownerId) 
    : base("Dewgong", "Water/Ice", 90, 70, 80, 70, 95, 70, ownerId, 30, "Thick Fat")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public Dewgong(Seel seel)
    : base("Dewgong", "Water/Ice", 90, 70, 80, 70, 95, 70, seel.OwnerId ?? "Unknown", 30, "Thick Fat")
    {
        Id = seel.Id;
        Level = 1;
        Nickname = seel.Nickname;
        Experience = seel.Experience;
        HpIV = seel.HpIV;
        AttackIV = seel.AttackIV;
        SpecialAttackIV = seel.SpecialAttackIV;
        DefenseIV = seel.DefenseIV;
        SpecialDefenseIV = seel.SpecialDefenseIV;
        SpeedIV = seel.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        await session.GetChoiceAsync($"{Nickname} is already at its final evolution stage.");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}