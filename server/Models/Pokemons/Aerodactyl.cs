using Server;
using Database;
namespace PokemonPocket;

public class Aerodactyl : PokemonMaster
{
    public override float HealthOverride {get;set;} = 80;
    public override string? Requirements { get; set; } = "Unevolvable";
    
    private Aerodactyl() { } //For EF Core
    public Aerodactyl(string nickname, string ownerId) 
    : base("Aerodactyl", "Rock/Flying", 80, 105, 65, 60, 75, 130, ownerId, 20, "Pressure")
    {
        Nickname = nickname;
        SkillPool = "Wing Attack, Supersonic, Bite, Take Down, Agility, Hyper Beam, Toxic, Rage, Earthquake, Mimic, Double-Edge, Skull Bash, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Aerodactyl(float HP, string nickname, string ownerId, int exp)
    : base("Aerodactyl", "Rock/Flying", HP, 105, 65, 60, 75, 130, ownerId, 20, "Pressure")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Wing Attack, Supersonic, Bite, Take Down, Agility, Hyper Beam, Toxic, Rage, Earthquake, Mimic, Double-Edge, Skull Bash, Rest, Substitute";

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
