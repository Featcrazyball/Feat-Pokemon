using Server;
namespace PokemonPocket;

public class Exeggutor : PokemonMaster
{
    private Exeggutor() { } //For EF Core
    public Exeggutor(string nickname, string ownerId) 
    : base("Exeggutor", "Grass/Psychic", 95, 95, 85, 125, 75, 55, ownerId, 30, "Chlorophyll")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public Exeggutor(Exeggcute exeggcute)
    : base("Exeggcute", "Grass/Psychic", 95, 95, 85, 125, 75, 55, exeggcute.OwnerId ?? "Unknown", 30, "Chlorophyll")
    {
        Id = exeggcute.Id;
        Level = 1;
        Nickname = exeggcute.Nickname;
        Experience = exeggcute.Experience;
        HpIV = exeggcute.HpIV;
        AttackIV = exeggcute.AttackIV;
        SpecialAttackIV = exeggcute.SpecialAttackIV;
        DefenseIV = exeggcute.DefenseIV;
        SpecialDefenseIV = exeggcute.SpecialDefenseIV;
        SpeedIV = exeggcute.SpeedIV;
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