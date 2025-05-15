using Server;
using Database;
namespace PokemonPocket;

public class MrMime : PokemonMaster
{
    public override float HealthOverride {get;set;} = 40;
    public override string? Requirements { get; set; } = "Unevolvable";
    private MrMime() { } //For EF Core
    public MrMime(string nickname, string ownerId) 
    : base("MrMime", "Psychic", 40, 45, 65, 100, 120, 90, ownerId, 15, "Soundproof")
    {
        Nickname = nickname;
        SkillPool = "Confusion, Barrier, Light Screen, DoubleSlap, Meditate, Substitute, Psychic, Seismic Toss, Thunderbolt, Thunder Wave, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Psywave";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public MrMime(float HP, string nickname, string ownerId, int exp)
    : base("MrMime", "Psychic", HP, 45, 65, 100, 120, 90, ownerId, 15, "Soundproof")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Confusion, Barrier, Light Screen, DoubleSlap, Meditate, Substitute, Psychic, Seismic Toss, Thunderbolt, Thunder Wave, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Psywave";

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
