using Database;
using Server;
namespace PokemonPocket;

public class Ivysaur : PokemonMaster
{
    public override float HealthOverride {get;set;} = 60;
    public override string? Requirements { get; set; } = "Level 32";
    public override string? EvolvesTo {get;set;} = "Venusaur";
    private Ivysaur() { } //For EF Core
    public Ivysaur(string nickname, string ownerId) 
    : base("Ivysaur", "Grass/Poison", 60, 62, 63, 80, 80, 60, ownerId, 20, "Water Burst")
    {
        Nickname = nickname;
        SkillPool = "Tackle, Growl, Leech Seed, Vine Whip, Poison Powder, Sleep Powder, Razor Leaf, Growth, Solar Beam, Body Slam, Take Down, Double-Edge, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Cut";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Ivysaur(float HP, string nickname, string ownerId, int exp)
    : base("Ivysaur", "Grass/Poison", HP, 62, 63, 80, 80, 60, ownerId, 20, "Water Burst")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Tackle, Growl, Leech Seed, Vine Whip, Poison Powder, Sleep Powder, Razor Leaf, Growth, Solar Beam, Body Slam, Take Down, Double-Edge, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Cut";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Ivysaur(Bulbasaur bulbasaur)
    : base("Ivysaur", "Grass/Poison", 100, 62, 63, 80, 80, 60, bulbasaur.OwnerId ?? "Unknown", 20, "Water Burst")
    {
        Id = bulbasaur.Id;
        Nickname = bulbasaur.Nickname;
        Level = 1;
        Experience = 0;
        HpIV = bulbasaur.HpIV;
        AttackIV = bulbasaur.AttackIV;
        SpecialAttackIV = bulbasaur.SpecialAttackIV;
        DefenseIV = bulbasaur.DefenseIV;
        SpecialDefenseIV = bulbasaur.SpecialDefenseIV;
        SpeedIV = bulbasaur.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Tackle, Growl, Leech Seed, Vine Whip, Poison Powder, Sleep Powder, Razor Leaf, Growth, Solar Beam, Body Slam, Take Down, Double-Edge, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Cut";

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
        if (Level >= 32) {
            using (var context = new DatabaseContext())
            {
                var venusaur = new Venusaur(this);
                venusaur.MaxHealth = venusaur.HealthOverride;
                venusaur.EvolveLevelUp(Level-1);

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(venusaur);
                foreach (var skill in venusaur.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Ivysaur to a Venusaur!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override async Task GodEvolve(ClientSession session)
    {
        using (var context = new DatabaseContext())
        {
            var venusaur = new Venusaur(this);
            venusaur.MaxHealth = venusaur.HealthOverride;
            venusaur.EvolveLevelUp(Level-1);

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(venusaur);
            foreach (var skill in venusaur.Skills)
            {
                context.Skills.Add(skill);
            }

            // Remove previous and add new Pokemon
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Ivysaur to a Venusaur!");
    }

    public override float calculateDamage(float SkillDamage)
    {
        return 2 * SkillDamage;
    }
}
