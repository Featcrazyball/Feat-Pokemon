using Server;
namespace PokemonPocket;

public class Rapidash : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Rapidash() { } //For EF Core
    public Rapidash(string nickname, string ownerId) 
    : base("Rapidash", "Fire", 65, 100, 70, 80, 80, 105, ownerId, 40, "Flame Body")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public Rapidash(Ponyta ponyta)
    : base("Rapidash", "Fire", 65, 100, 70, 80, 80, 105, ponyta.OwnerId ?? "Unknown", 40, "Flame Body")
    {
        Id = ponyta.Id;
        Level = 1;
        Nickname = ponyta.Nickname;
        Experience = ponyta.Experience;
        HpIV = ponyta.HpIV;
        AttackIV = ponyta.AttackIV;
        SpecialAttackIV = ponyta.SpecialAttackIV;
        DefenseIV = ponyta.DefenseIV;
        SpecialDefenseIV = ponyta.SpecialDefenseIV;
        SpeedIV = ponyta.SpeedIV;
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