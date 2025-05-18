using Server;
using Database;
namespace PokemonPocket;

public class Lickitung : PokemonMaster
{
    public override float HealthOverride {get;set;} = 90;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Lickitung() { } //For EF Core
    public Lickitung(string nickname, string ownerId) 
    : base("Lickitung", "Normal", 90, 55, 75, 60, 75, 30, ownerId, 20, "Oblivious")
    {
        Nickname = nickname;
        SkillPool = "Wrap, Supersonic, Stomp, Disable, Defense Curl, Slam, Screech, Body Slam, Earthquake, Hyper Beam, Surf, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Lickitung(float HP, string nickname, string ownerId, int exp)
    : base("Lickitung", "Normal", HP, 55, 75, 60, 75, 30, ownerId, 20, "Oblivious")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Wrap, Supersonic, Stomp, Disable, Defense Curl, Slam, Screech, Body Slam, Earthquake, Hyper Beam, Surf, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public override async Task GodEvolve(ClientSession session)
    {
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is already at its final evolution stage.");
    }

    public override async Task Evolve(ClientSession session)
    {
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is already at its final evolution stage.");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}
