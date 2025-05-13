using Server;
using Database;
namespace PokemonPocket;

public class Lapras : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Lapras() { } //For EF Core
    public Lapras(string nickname, string ownerId) 
    : base("Lapras", "Water/Ice", 130, 85, 80, 85, 95, 60, ownerId, 30, "Water Absorb")
    {
        Nickname = nickname;
        SkillPool = "Water Gun, Sing, Mist, Body Slam, Confuse Ray, Ice Beam, Hydro Pump, Blizzard, Thunderbolt, Psychic, Surf, Strength, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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