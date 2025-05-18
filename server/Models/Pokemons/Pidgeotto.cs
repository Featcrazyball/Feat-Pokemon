using Database;
using Server;
namespace PokemonPocket;

public class Pidgeotto : PokemonMaster
{
    public override float HealthOverride {get;set;} = 63;
    public override string? Requirements { get; set; } = "Level 36";
    public override string? EvolvesTo {get;set;} = "Pidgeot";
    private Pidgeotto() { } //For EF Core
    public Pidgeotto(string nickname, string ownerId) 
    : base("Pidgeotto", "Normal/Flying", 63, 60, 55, 50, 50, 71, ownerId, 25, "Gust")
    {
        Nickname = nickname;
        SkillPool = "Gust, Sand Attack, Quick Attack, Whirlwind, Wing Attack, Agility, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Fly";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Pidgeotto(float HP, string nickname, string ownerId, int exp)
    : base("Pidgeotto", "Normal/Flying", HP, 60, 55, 50, 50, 71, ownerId, 25, "Gust")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Gust, Sand Attack, Quick Attack, Whirlwind, Wing Attack, Agility, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Fly";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Pidgeotto(Pidgey pidgey)
    : base("Pidgeotto", "Normal/Flying", 100, 60, 55, 50, 50, 71, pidgey.OwnerId ?? "Unknown", 25, "Gust")
    {
        Id = pidgey.Id;
        Level = 1;
        Nickname = pidgey.Nickname;
        Experience = 0;
        HpIV = pidgey.HpIV;
        AttackIV = pidgey.AttackIV;
        SpecialAttackIV = pidgey.SpecialAttackIV;
        DefenseIV = pidgey.DefenseIV;
        SpecialDefenseIV = pidgey.SpecialDefenseIV;
        SpeedIV = pidgey.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Gust, Sand Attack, Quick Attack, Whirlwind, Wing Attack, Agility, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Fly";

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
        if (Level >= 36) {
            using (var context = new DatabaseContext())
            {
                var pidgeot = new Pidgeot(this);
                pidgeot.MaxHealth = pidgeot.HealthOverride;
                pidgeot.EvolveLevelUp(Level-1); // Level up to 36

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(pidgeot);
                foreach (var skill in pidgeot.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Pidgeotto to a Pidgeot!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override async Task GodEvolve(ClientSession session)
    {
        using (var context = new DatabaseContext())
        {
            var pidgeot = new Pidgeot(this);
            pidgeot.MaxHealth = pidgeot.HealthOverride;
            pidgeot.EvolveLevelUp(Level-1); // Level up to 36

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(pidgeot);
            foreach (var skill in pidgeot.Skills)
            {
                context.Skills.Add(skill);
            }

            // Remove previous and add new Pokemon
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Pidgeotto to a Pidgeot!");
    }

    public override float calculateDamage(float SkillDamage)
    {
        return SkillDamage;
    }
}
