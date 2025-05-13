using Database;
using Server;
namespace PokemonPocket;

public class Poliwhirl : PokemonMaster
{
    public override string? Requirements { get; set; } = "1 Water Stone";
    public override string? EvolvesTo {get;set;} = "Poliwrath";
    private Poliwhirl() { } //For EF Core
    public Poliwhirl(string nickname, string ownerId) 
    : base("Poliwhirl", "Water", 65, 65, 65, 50, 50, 90, ownerId, 25, "Water Absorb")
    {
        Nickname = nickname;
        SkillPool = "Bubble, Hypnosis, Water Gun, Double Slap, Body Slam, Amnesia, Hydro Pump, Toxic, Take Down, Double-Edge, Ice Beam, Blizzard, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Surf, Strength";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }
    
    public Poliwhirl(Poliwag poliwag)
    : base("Poliwhirl", "Water", 65, 65, 65, 50, 50, 90, poliwag.OwnerId ?? "Unknown", 25, "Water Absorb")
    {
        Id = poliwag.Id;
        Level = 1;
        Nickname = poliwag.Nickname;
        Experience = poliwag.Experience;
        HpIV = poliwag.HpIV;
        AttackIV = poliwag.AttackIV;
        SpecialAttackIV = poliwag.SpecialAttackIV;
        DefenseIV = poliwag.DefenseIV;
        SpecialDefenseIV = poliwag.SpecialDefenseIV;
        SpeedIV = poliwag.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Bubble, Hypnosis, Water Gun, DoubleSlap, Body Slam, Amnesia, Hydro Pump, Toxic, Take Down, Double-Edge, Ice Beam, Blizzard, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Surf, Strength";


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
        using (var context = new DatabaseContext())
        {
            var item = context.Items.FirstOrDefault(i => i.Name == "Water Stone" && i.OwnerId == OwnerId);
            if (item != null) {
                context.Items.Remove(item);
            } else {
                await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} needs a Water Stone to evolve!");
                return;
            }

            var poliwrath = new Poliwrath(this);
            poliwrath.EvolveLevelUp(Level-1); // Level up to current level

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(poliwrath);
            foreach (var skill in poliwrath.Skills)
            {
                context.Skills.Add(skill);
            }

            // Remove previous and add new Pokemon
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Poliwhirl to a Poliwrath!");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}