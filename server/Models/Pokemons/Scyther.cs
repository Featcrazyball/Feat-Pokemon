using Server;
using Database;
namespace PokemonPocket;

public class Scyther : PokemonMaster
{
    public override float HealthOverride {get;set;} = 70;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Scyther() { } //For EF Core
    public Scyther(string nickname, string ownerId) 
    : base("Scyther", "Bug/Flying", 70, 110, 80, 55, 80, 105, ownerId, 20, "Swarm")
    {
        Nickname = nickname;
        SkillPool = "Quick Attack, Leer, Focus Energy, Agility, Slash, Swords Dance, Hyper Beam, Body Slam, Seismic Toss, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Scyther(float HP, string nickname, string ownerId, int exp)
    : base("Scyther", "Bug/Flying", HP, 110, 80, 55, 80, 105, ownerId, 20, "Swarm")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Quick Attack, Leer, Focus Energy, Agility, Slash, Swords Dance, Hyper Beam, Body Slam, Seismic Toss, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
