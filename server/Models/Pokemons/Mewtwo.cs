using System.Data.SQLite;
using Server;
using Database;
namespace PokemonPocket;

public class Mewtwo : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Mewtwo() { } //For EF Core
    public Mewtwo(string nickname, string ownerId) 
    : base("Mewtwo", "Psychic", 106, 110, 90, 154, 90, 130, ownerId, 70, "Pressure")
    {
        Nickname = nickname;
        SkillPool = "Confusion, Disable, Swift, Psychic, Barrier, Recover, Mist, Amnesia, Reflect, Hyper Beam, Thunderbolt, Blizzard, Fire Blast, Toxic, Mimic, Double Team, Bide, Rest, Substitute";

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