using Server;
using Database;
namespace PokemonPocket;

public class Articuno : PokemonMaster
{
    public override float HealthOverride { get; set; } = 90;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Articuno() { } //For EF Core
    public Articuno(string nickname, string ownerId)
    : base("Articuno", "Ice/Flying", 90, 85, 100, 95, 125, 85, ownerId, 25, "Pressure")
    {
        Nickname = nickname;
        SkillPool = "Peck, Ice Beam, Blizzard, Agility, Mist, Toxic, Rage, Mimic, Double Team, Reflect, Bide, Swift, Sky Attack, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills)
            {
                Skills.Add(skill);
            };
        }
    }

    public Articuno(float HP, string nickname, string ownerId, int exp)
    : base("Articuno", "Ice/Flying", HP, 85, 100, 95, 125, 85, ownerId, 25, "Pressure")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Peck, Ice Beam, Blizzard, Agility, Mist, Toxic, Rage, Mimic, Double Team, Reflect, Bide, Swift, Sky Attack, Rest, Substitute";

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
    public override async Task GodEvolve(ClientSession session)
    {
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is already at its final evolution stage.");
    }
}
