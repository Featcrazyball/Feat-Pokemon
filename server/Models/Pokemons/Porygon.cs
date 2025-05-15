using Server;
using Database;
namespace PokemonPocket;

public class Porygon : PokemonMaster
{
    public override float HealthOverride {get;set;} = 65;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Porygon() { } //For EF Core
    public Porygon(string nickname, string ownerId) 
    : base("Porygon", "Normal", 65, 60, 70, 85, 75, 40, ownerId, 15, "Trace")
    {
        Nickname = nickname;
        SkillPool = "Tackle, Sharpen, Conversion, Psybeam, Recover, Agility, Tri Attack, Toxic, Thunderbolt, Thunder, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Porygon(float HP, string nickname, string ownerId, int exp)
    : base("Porygon", "Normal", HP, 60, 70, 85, 75, 40, ownerId, 15, "Trace")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Tackle, Sharpen, Conversion, Psybeam, Recover, Agility, Tri Attack, Toxic, Thunderbolt, Thunder, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public override async Task Evolve(ClientSession session)
    {
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is already at its final evolution stage.");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}
