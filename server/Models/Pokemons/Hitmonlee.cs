using Server;
using Database;
namespace PokemonPocket;

public class Hitmonlee : PokemonMaster
{
    public override float HealthOverride {get;set;} = 50;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Hitmonlee() { } //For EF Core
    public Hitmonlee(string nickname, string ownerId) 
    : base("Hitmonlee", "Fighting", 50, 120, 53, 35, 110, 87, ownerId, 20, "Limber")
    {
        Nickname = nickname;
        SkillPool = "Double Kick, Meditate, Rolling Kick, Jump Kick, Focus Energy, High Jump Kick, Mega Kick, Seismic Toss, Body Slam, Take Down, Double-Edge, Submission, Counter, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute";


        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Hitmonlee(float HP, string nickname, string ownerId, int exp)
    : base("Hitmonlee", "Fighting", HP, 120, 53, 35, 110, 87, ownerId, 20, "Limber")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Double Kick, Meditate, Rolling Kick, Jump Kick, Focus Energy, High Jump Kick, Mega Kick, Seismic Toss, Body Slam, Take Down, Double-Edge, Submission, Counter, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
