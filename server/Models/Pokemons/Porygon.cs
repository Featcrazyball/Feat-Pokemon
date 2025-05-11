using Server;
using Database;
namespace PokemonPocket;

public class Porygon : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Porygon() { } //For EF Core
    public Porygon(string nickname, string ownerId) 
    : base("Porygon", "Normal", 65, 60, 70, 85, 75, 40, ownerId, 15, "Trace")
    {
        Nickname = nickname;
        SkillPool = "Tackle, Sharpen, Conversion, Psybeam, Recover, Agility, Tri Attack, Toxic, Thunderbolt, Thunder, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
        await session.SendMessageAsync($"{Nickname} is already at its final evolution stage.");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}