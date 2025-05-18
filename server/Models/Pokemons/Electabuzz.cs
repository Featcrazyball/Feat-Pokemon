using Server;
using Database;
namespace PokemonPocket;

public class Electabuzz : PokemonMaster
{
    public override float HealthOverride {get;set;} = 65;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Electabuzz() { } //For EF Core
    public Electabuzz(string nickname, string ownerId) 
    : base("Electabuzz", "Electric", 65, 83, 57, 95, 85, 105, ownerId, 30, "Static")
    {
        Nickname = nickname;
        SkillPool = "Quick Attack, Leer, Thunder Shock, Screech, Thunder Punch, Thunder, Toxic, Body Slam, Take Down, Double-Edge, Seismic Toss, Rage, Thunderbolt, Thunder Wave, Psychic, Mimic, Double Team, Reflect, Bide, Swift, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Electabuzz(float HP, string nickname, string ownerId, int exp)
    : base("Electabuzz", "Electric", HP, 83, 57, 95, 85, 105, ownerId, 30, "Static")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Quick Attack, Leer, Thunder Shock, Screech, Thunder Punch, Thunder, Toxic, Body Slam, Take Down, Double-Edge, Seismic Toss, Rage, Thunderbolt, Thunder Wave, Psychic, Mimic, Double Team, Reflect, Bide, Swift, Rest, Substitute";

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

    public override async Task GodEvolve(ClientSession session)
    {
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is already at its final evolution stage.");
    }

    public override float calculateDamage(float SkillDamage)
    {
        return SkillDamage;
    }
}
