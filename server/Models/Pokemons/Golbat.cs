using Server;
namespace PokemonPocket;

public class Golbat : PokemonMaster
{
    private Golbat() { } //For EF Core
    public Golbat(string nickname, string ownerId) 
    : base("Golbat", "Poison/Flying", 75, 80, 70, 65, 75, 90, ownerId, 20, "Inner Focus")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public Golbat(Zubat zubat)
    : base("Golbat", "Poison/Flying", 75, 80, 70, 65, 75, 90, zubat.OwnerId ?? "Unknown", 20, "Inner Focus")
    {
        Id = zubat.Id;
        Level = 1;
        Nickname = zubat.Nickname;
        Experience = zubat.Experience;
        HpIV = zubat.HpIV;
        AttackIV = zubat.AttackIV;
        SpecialAttackIV = zubat.SpecialAttackIV;
        DefenseIV = zubat.DefenseIV;
        SpecialDefenseIV = zubat.SpecialDefenseIV;
        SpeedIV = zubat.SpeedIV;
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