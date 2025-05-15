using Server;
using Database;
namespace PokemonPocket;

public class Ditto : PokemonMaster
{
    public override float HealthOverride {get;set;} = 48;
    public override string? Requirements { get; set; } = "Unevolvable";
    
    private Ditto() { } //For EF Core
    public Ditto(string nickname, string ownerId) 
    : base("Ditto", "Normal", 48, 48, 48, 48, 48, 48, ownerId, 20, "Limber")
    {
        Nickname = nickname;
        SkillPool = "Transform";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Ditto(float HP, string nickname, string ownerId, int exp)
    : base("Ditto", "Normal", HP, 48, 48, 48, 48, 48, ownerId, 20, "Limber")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Transform";

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
