using Server;
namespace PokemonPocket;

public class Dragonite : PokemonMaster
{
    private Dragonite() { } //For EF Core
    public Dragonite(string nickname, string ownerId) 
    : base("Dragonite", "Dragon", 91, 134, 95, 100, 100, 80, ownerId, 60, "Inner Focus")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public Dragonite(Dragonair dragonair)
    : base("Dragonite", "Dragon", 91, 134, 95, 100, 100, 80, dragonair.OwnerId?? "Unknown", 60, "Inner Focus")
    {
        Id = dragonair.Id;
        Level = 1;
        Nickname = dragonair.Nickname;
        Experience = dragonair.Experience;
        HpIV = dragonair.HpIV;
        AttackIV = dragonair.AttackIV;
        SpecialAttackIV = dragonair.SpecialAttackIV;
        DefenseIV = dragonair.DefenseIV;
        SpecialDefenseIV = dragonair.SpecialDefenseIV;
        SpeedIV = dragonair.SpeedIV;
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