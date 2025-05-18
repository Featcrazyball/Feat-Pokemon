using Server;
using Database;
namespace PokemonPocket;

public class Pinsir : PokemonMaster
{
    public override float HealthOverride {get;set;} = 65;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Pinsir() { } //For EF Core
    public Pinsir(string nickname, string ownerId) 
    : base("Pinsir", "Bug", 65, 125, 100, 55, 70, 85, ownerId, 20, "Hyper Cutter")
    {
        Nickname = nickname;
        SkillPool = "Seismic Toss, Guillotine, Focus Energy, Harden, Slash, Swords Dance, Body Slam, Take Down, Double-Edge, Submission, Hyper Beam, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Strength";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Pinsir(float HP, string nickname, string ownerId, int exp)
    : base("Pinsir", "Bug", HP, 125, 100, 55, 70, 85, ownerId, 20, "Hyper Cutter")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Seismic Toss, Guillotine, Focus Energy, Harden, Slash, Swords Dance, Body Slam, Take Down, Double-Edge, Submission, Hyper Beam, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Strength";

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
