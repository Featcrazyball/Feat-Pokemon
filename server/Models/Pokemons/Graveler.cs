using Database;
using Server;
namespace PokemonPocket;

public class Graveler : PokemonMaster
{
    public override float HealthOverride {get;set;} = 55;
    public override string? Requirements { get; set; } = "Trade";
    public override string? EvolvesTo {get;set;} = "Golem";
    private Graveler() { } //For EF Core
    public Graveler(string nickname, string ownerId) 
    : base("Graveler", "Rock/Ground", 55, 95, 115, 45, 45, 35, ownerId, 25, "Sturdy")
    {
        Nickname = nickname;
        SkillPool = "Tackle, Defense Curl, Rock Throw, Self-Destruct, Harden, Earthquake, Explosion, Toxic, Body Slam, Take Down, Double-Edge, Seismic Toss, Rage, Mimic, Double Team, Reflect, Bide, Fire Blast, Rest, Substitute, Strength";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Graveler(float HP, string nickname, string ownerId, int exp)
    : base("Graveler", "Rock/Ground", HP, 95, 115, 45, 45, 35, ownerId, 25, "Sturdy")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Tackle, Defense Curl, Rock Throw, Self-Destruct, Harden, Earthquake, Explosion, Toxic, Body Slam, Take Down, Double-Edge, Seismic Toss, Rage, Mimic, Double Team, Reflect, Bide, Fire Blast, Rest, Substitute, Strength";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Graveler(Geodude geodude)
    : base("Graveler", "Rock/Ground", 100, 95, 115, 45, 45, 35, geodude.OwnerId ?? "Unknown", 25, "Sturdy")
    {
        Id = geodude.Id;
        Level = 1;
        Nickname = geodude.Nickname;
        Experience = 0;
        HpIV = geodude.HpIV;
        AttackIV = geodude.AttackIV;
        SpecialAttackIV = geodude.SpecialAttackIV;
        DefenseIV = geodude.DefenseIV;
        SpecialDefenseIV = geodude.SpecialDefenseIV;
        SpeedIV = geodude.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Tackle, Defense Curl, Rock Throw, Self-Destruct, Harden, Earthquake, Explosion, Toxic, Body Slam, Take Down, Double-Edge, Seismic Toss, Rage, Mimic, Double Team, Reflect, Bide, Fire Blast, Rest, Substitute, Strength";

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
                var golem = new Golem(this);
                golem.MaxHealth = golem.HealthOverride;
                golem.EvolveLevelUp(Level-1); 

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(golem);
                foreach (var skill in golem.Skills)
                {
                    context.Skills.Add(skill);
                }

                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Gravler to a Golem!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override async Task GodEvolve(ClientSession session)
    {
        using (var context = new DatabaseContext())
        {
            var golem = new Golem(this);
            golem.MaxHealth = golem.HealthOverride;
            golem.EvolveLevelUp(Level-1); 

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(golem);
            foreach (var skill in golem.Skills)
            {
                context.Skills.Add(skill);
            }

            context.SaveChanges();
        }
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Gravler to a Golem!");
    }


    public override float calculateDamage(float SkillDamage)
    {
        return SkillDamage;
    }
}
