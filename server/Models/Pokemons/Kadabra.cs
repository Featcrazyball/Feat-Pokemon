using Database;
using Server;
namespace PokemonPocket;

public class Kadabra : PokemonMaster
{
    public override string? Requirements { get; set; } = "Trade";
    public override string? EvolvesTo {get;set;} = "Alakazam";
    private Kadabra() { } //For EF Core
    public Kadabra(string nickname, string ownerId) 
    : base("Kadabrah", "Psychic", 40, 35, 30, 120, 70, 105, ownerId, 50, "Synchronize")
    {
        Nickname = nickname;
        SkillPool = "Teleport, Confusion, Disable, Psybeam, Recover, Psychic, Reflect, Toxic, Body Slam, Take Down, Double-Edge, Seismic Toss, Counter, Rage, Thunder Wave, Mimic, Double Team, Bide, Metronome, Skull Bash, Rest, Psywave, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Kadabra(Abra abra) 
    : base("Kadabra", "Psychic", 40, 35, 30, 120, 70, 105, abra.OwnerId ?? "Unknown", 50, "Synchronize")
    {
        Id = abra.Id;
        Level = 1;
        Nickname = abra.Nickname;
        Experience = abra.Experience;
        HpIV = abra.HpIV;
        AttackIV = abra.AttackIV;
        SpecialAttackIV = abra.SpecialAttackIV;
        DefenseIV = abra.DefenseIV;
        SpecialDefenseIV = abra.SpecialDefenseIV;
        SpeedIV = abra.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Teleport, Confusion, Disable, Psybeam, Recover, Psychic, Reflect, Toxic, Body Slam, Take Down, Double-Edge, Seismic Toss, Counter, Rage, Thunder Wave, Mimic, Double Team, Bide, Metronome, Skull Bash, Rest, Psywave, Substitute";
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
        if (Level >= 1) {
            using (var context = new DatabaseContext())
            {
                var alakazam = new Alakazam(this);
                alakazam.EvolveLevelUp(Level-1); // Level up to current level

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(alakazam);
                foreach (var skill in alakazam.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Abra to a Kadabra!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}