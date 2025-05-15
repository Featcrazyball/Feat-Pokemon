using Server;
using Database;
namespace PokemonPocket;

public class Chansey : PokemonMaster
{
    public override float HealthOverride {get;set;} = 250;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Chansey() { } //For EF Core
    public Chansey(string nickname, string ownerId) 
    : base("Chansey", "Normal", 250, 5, 5, 35, 105, 50, ownerId, 30, "Natural Cure")
    {
        Nickname = nickname;
        SkillPool = "Pound, Double-Edge, Sing, Growl, Minimize, Defense Curl, Light Screen, DoubleSlap, Soft-boiled, Egg Bomb, Take Down, Seismic Toss, Counter, Toxic, Rage, Psychic, Teleport, Mimic, Double Team, Reflect, Bide, Metronome, Thunderbolt, Thunder, Fire Blast, Rest, Psywave, Substitute";

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
