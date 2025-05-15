using Server;
using Database;
namespace PokemonPocket;

public class Mew : PokemonMaster
{
    public override float HealthOverride {get;set;} = 100;
    public override string? Requirements { get; set; } = "Unevolvable";
    private Mew() { } //For EF Core
    public Mew(string nickname, string ownerId) 
    : base("Mew", "Psychic", 100, 100, 100, 100, 100, 100, ownerId, 30, "Synchronize")
    {
        Nickname = nickname;
        SkillPool = "Pound, Transform, Mega Punch, Metronome, Cut, Fly, Surf, Strength, Flash, Razor Wind, Swords Dance, Whirlwind, Mega Kick, Toxic, Horn Drill, Body Slam, Take Down, Double-Edge, Bubble Beam, Water Gun, Ice Beam, Blizzard, Hyper Beam, Pay Day, Submission, Counter, Seismic Toss, Rage, Mega Drain, Solar Beam, Dragon Rage, Thunderbolt, Thunder, Earthquake, Fissure, Dig, Psychic, Teleport, Mimic, Double Team, Reflect, Bide, Metronome, Self-Destruct, Egg Bomb, Fire Blast, Swift, Skull Bash, Soft-Boiled, Dream Eater, Sky Attack, Rest, Thunder Wave, Psywave, Explosion, Rock Slide, Tri Attack, Substitute";

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
