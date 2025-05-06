using Server;
namespace PokemonPocket;

public class Seaking : PokemonMaster
{
    private Seaking() { } //For EF Core
    public Seaking(string nickname, string ownerId) 
    : base("Seaking", "Water", 80, 92, 65, 65, 80, 68, ownerId, 30, "Swift Swim")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public Seaking(Goldeen goldeen)
    : base("Seaking", "Water", 80, 92, 65, 65, 80, 68, goldeen.OwnerId ?? "Unknown", 30, "Swift Swim")
    {
        Id = goldeen.Id;
        Level = 1;
        Nickname = goldeen.Nickname;
        Experience = goldeen.Experience;
        HpIV = goldeen.HpIV;
        AttackIV = goldeen.AttackIV;
        SpecialAttackIV = goldeen.SpecialAttackIV;
        DefenseIV = goldeen.DefenseIV;
        SpecialDefenseIV = goldeen.SpecialDefenseIV;
        SpeedIV = goldeen.SpeedIV;
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