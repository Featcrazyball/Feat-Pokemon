using Server;
using Database;
namespace PokemonPocket;

public class Snorlax : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Snorlax() { } //For EF Core
    public Snorlax(string nickname, string ownerId) 
    : base("Snorlax", "Normal", 160, 110, 65, 65, 110, 30, ownerId, 30, "Immunity")
    {
        Nickname = nickname;
        SkillPool = "Headbutt, Amnesia, Rest, Body Slam, Hyper Beam, Earthquake, Surf, Strength, Seismic Toss, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
        await session.SendMessageAsync($"{Nickname == "None" ? Name : Nickname} is already at its final evolution stage.");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}