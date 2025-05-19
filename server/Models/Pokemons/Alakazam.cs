using Server;
using Database;
namespace PokemonPocket;

public class Alakazam : PokemonMaster
{
    public override float HealthOverride {get;set;} = 55;
    public override string? Requirements { get; set; } = "Unevolvable";

    private Alakazam() { } //For EF Core
    public Alakazam(string nickname, string ownerId) 
    : base("Alakazam", "Psychic", 55, 50, 45, 135, 95, 120, ownerId, 20, "Synchronize")
    {
        Nickname = nickname;
        SkillPool = "Confusion, Psybeam, Recover, Psychic, Reflect, Kinesis, Toxic, Seismic Toss, Rage, Hyper Beam, Counter, Mimic, Double Team, Bide, Metronome, Swift, Dream Eater, Rest, Psywave, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Alakazam(string ownerId) 
    : base("Alakazam", "Psychic", 100, 50, 45, 135, 95, 120, ownerId, 20, "Synchronize")
    {
        Nickname = "None";
        Experience = 0;
        SkillPool = "Confusion, Psybeam, Recover, Psychic, Reflect, Kinesis, Toxic, Seismic Toss, Rage, Hyper Beam, Counter, Mimic, Double Team, Bide, Metronome, Swift, Dream Eater, Rest, Psywave, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Alakazam(float HP, string nickname, string ownerId, int exp)
    : base("Alakazam", "Psychic", HP, 50, 45, 135, 95, 120, ownerId, 20, "Synchronize")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Confusion, Psybeam, Recover, Psychic, Reflect, Kinesis, Toxic, Seismic Toss, Rage, Hyper Beam, Counter, Mimic, Double Team, Bide, Metronome, Swift, Dream Eater, Rest, Psywave, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills)
            {
                Skills.Add(skill);
            }
            ;
        }
    }

    public Alakazam(Kadabra kadabra) 
    : base("Kadabra", "Psychic", 100, 50, 45, 135, 95, 120, kadabra.OwnerId ?? "Unknown", 50, "Synchronize")
    {
        Id = kadabra.Id;
        Level = 1;
        Nickname = kadabra.Nickname;
        Experience = 0;
        HpIV = kadabra.HpIV;
        AttackIV = kadabra.AttackIV;
        SpecialAttackIV = kadabra.SpecialAttackIV;
        DefenseIV = kadabra.DefenseIV;
        SpecialDefenseIV = kadabra.SpecialDefenseIV;
        SpeedIV = kadabra.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        
        SkillPool = "Confusion, Psybeam, Recover, Psychic, Reflect, Kinesis, Toxic, Seismic Toss, Rage, Hyper Beam, Counter, Mimic, Double Team, Bide, Metronome, Swift, Dream Eater, Rest, Psywave, Substitute";

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
    
    public override async Task GodEvolve(ClientSession session)
    {
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is already at its final evolution stage.");
    }

    public override float calculateDamage(float SkillDamage)
    {
        return SkillDamage;
    }
}
