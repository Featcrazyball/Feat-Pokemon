using Server;
namespace PokemonPocket;

public class Persian : PokemonMaster
{
    private Persian() { } //For EF Core
    public Persian(string nickname, string ownerId) 
    : base("Persian", "Normal", 65, 70, 60, 65, 65, 115, ownerId, 34, "Limber")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public Persian(Meowth meowth)
    : base("Persian", "Normal", 65, 70, 60, 65, 65, 115, meowth.OwnerId ?? "Unknown", 34, "Limber")
    {
        Id = meowth.Id;
        Level = 1;
        Nickname = meowth.Nickname;
        Experience = meowth.Experience;
        HpIV = meowth.HpIV;
        AttackIV = meowth.AttackIV;
        SpecialAttackIV = meowth.SpecialAttackIV;
        DefenseIV = meowth.DefenseIV;
        SpecialDefenseIV = meowth.SpecialDefenseIV;
        SpeedIV = meowth.SpeedIV;
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