using Server;
using Database;
namespace PokemonPocket;

public class Kangaskhan : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Kangaskhan() { } //For EF Core
    public Kangaskhan(string nickname, string ownerId) 
    : base("Kangaskhan", "Normal", 105, 95, 80, 40, 80, 90, ownerId, 45, "Early Bird")
    {
        Nickname = nickname;
        SkillPool = "Comet Punch, Rage, Bite, Tail Whip, Mega Punch, Leer, Dizzy Punch, Toxic, Body Slam, Take Down, Double-Edge, Counter, Seismic Toss, Earthquake, Fissure, Rage, Thunderbolt, Thunder, Mimic, Double Team, Reflect, Bide, Fire Blast, Skull Bash, Rest, Substitute, Surf, Strength";

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