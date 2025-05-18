using Database;
using Server;
namespace PokemonPocket;

public class Clefairy : PokemonMaster
{
    public override float HealthOverride {get;set;} = 70;
    public override string? Requirements { get; set; } = "1 Moon Stone";
    public override string? EvolvesTo {get;set;} = "Clefable";
    private Clefairy() { } //For EF Core
    public Clefairy(string nickname, string ownerId) 
    : base("Clefairy", "Fairy", 70, 45, 48, 60, 65, 35, ownerId, 10, "Cute Charm")
    {
        Nickname = nickname;
        SkillPool = "Pound, Growl, Sing, Double Slap, Minimize, Metronome, Defense Curl, Light Screen, Solar Beam, Thunderbolt, Thunder, Psychic, Teleport, Seismic Toss, Counter, Toxic, Body Slam, Take Down, Double-Edge, Submission, Rage, Dig, Mimic, Double Team, Reflect, Bide, Fire Blast, Swift, Skull Bash, Rest, Psywave, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Clefairy(float HP, string nickname, string ownerId, int exp)
    : base("Clefairy", "Fairy", HP, 45, 48, 60, 65, 35, ownerId, 10, "Cute Charm")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Pound, Growl, Sing, Double Slap, Minimize, Metronome, Defense Curl, Light Screen, Solar Beam, Thunderbolt, Thunder, Psychic, Teleport, Seismic Toss, Counter, Toxic, Body Slam, Take Down, Double-Edge, Submission, Rage, Dig, Mimic, Double Team, Reflect, Bide, Fire Blast, Swift, Skull Bash, Rest, Psywave, Substitute";

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
            var item = context.Items.FirstOrDefault(i => i.Name == "Moon Stone" && i.OwnerId == OwnerId);
            if (item != null) {
                context.Items.Remove(item);
            } else {
                await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} needs a Moon Stone to evolve!");
                return;
            }

            var clefable = new Clefable(this);
                clefable.MaxHealth = clefable.HealthOverride;
            clefable.EvolveLevelUp(Level-1); // Level up to current level

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            // Clean up skills from the old Pokemon to prevent orphaned records
            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(clefable);
            foreach (var skill in clefable.Skills)
            {
                context.Skills.Add(skill);
            }
            
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Clefairy to a Clefable!");
    }

    public override async Task GodEvolve(ClientSession session)
    {
        using (var context = new DatabaseContext())
        {
            var clefable = new Clefable(this);
            clefable.MaxHealth = clefable.HealthOverride;
            clefable.EvolveLevelUp(Level-1); // Level up to current level

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            // Clean up skills from the old Pokemon to prevent orphaned records
            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(clefable);
            foreach (var skill in clefable.Skills)
            {
                context.Skills.Add(skill);
            }
            
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Clefairy to a Clefable!");
    }

    public override float calculateDamage(float SkillDamage)
    {
        return SkillDamage;
    }
}
