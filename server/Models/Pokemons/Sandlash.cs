using Server;
namespace PokemonPocket;

public class Sandslash : PokemonMaster
{
    private Sandslash() { } //For EF Core
    public Sandslash(string nickname, string ownerId) 
    : base("Sandslash", "Ground", 75, 100, 110, 45, 55, 65, ownerId, 25, "Sand Attack")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public Sandslash(Sandshrew sandshrew)
    : base("Sandslash", "Ground", 75, 100, 110, 45, 55, 65, sandshrew.OwnerId ?? "Unknown", 25, "Sand Attack")
    {
        Id = sandshrew.Id;
        Level = 1;
        Nickname = sandshrew.Nickname;
        Experience = sandshrew.Experience;
        HpIV = sandshrew.HpIV;
        AttackIV = sandshrew.AttackIV;
        SpecialAttackIV = sandshrew.SpecialAttackIV;
        DefenseIV = sandshrew.DefenseIV;
        SpecialDefenseIV = sandshrew.SpecialDefenseIV;
        SpeedIV = sandshrew.SpeedIV;
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