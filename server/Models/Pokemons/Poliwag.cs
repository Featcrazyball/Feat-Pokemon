using Database;
using Server;
namespace PokemonPocket;

public class Poliwag : PokemonMaster
{
    public override float HealthOverride {get;set;} = 40;
    public override string? Requirements { get; set; } = "Level 25";
    public override string? EvolvesTo {get;set;} = "Poliwhirl";
    private Poliwag() { } //For EF Core
    public Poliwag(string nickname, string ownerId) 
    : base("Poliwag", "Water", 40, 50, 40, 40, 40, 90, ownerId, 16, "Water Absorb")
    {
        Nickname = nickname;
        SkillPool = "Bubble, Hypnosis, Water Gun, Double Slap, Body Slam, Amnesia, Hydro Pump, Toxic, Take Down, Double-Edge, Ice Beam, Blizzard, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Surf";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Poliwag(float HP, string nickname, string ownerId, int exp)
    : base("Poliwag", "Water", HP, 50, 40, 40, 40, 90, ownerId, 16, "Water Absorb")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Bubble, Hypnosis, Water Gun, Double Slap, Body Slam, Amnesia, Hydro Pump, Toxic, Take Down, Double-Edge, Ice Beam, Blizzard, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Surf";

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
        if (Level >= 25) {
            using (var context = new DatabaseContext())
            {
                var poliwhirl = new Poliwhirl(this);
                poliwhirl.MaxHealth = poliwhirl.HealthOverride;
                poliwhirl.EvolveLevelUp(Level-1); 

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(poliwhirl);
                foreach (var skill in poliwhirl.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Poliwag to a Poliwhirl!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}
