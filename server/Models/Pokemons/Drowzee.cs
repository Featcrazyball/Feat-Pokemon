using Database;
using Server;
namespace PokemonPocket;

public class Drowzee : PokemonMaster
{
    public override float HealthOverride {get;set;} = 60;
    public override string? Requirements { get; set; } = "Level 26";
    public override string? EvolvesTo {get;set;} = "Hypno";
    private Drowzee() { } //For EF Core
    public Drowzee(string nickname, string ownerId) 
    : base("Drowzee", "Psychic", 60, 48, 45, 43, 90, 42, ownerId, 20, "Insomnia")
    {
        Nickname = nickname;
        SkillPool = "Pound, Hypnosis, Disable, Confusion, Headbutt, Poison Gas, Psychic, Meditate, Toxic, Body Slam, Take Down, Double-Edge, Seismic Toss, Rage, Thunder Wave, Mimic, Double Team, Reflect, Bide, Metronome, Skull Bash, Dream Eater, Rest, Psywave, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Drowzee(float HP, string nickname, string ownerId, int exp)
    : base("Drowzee", "Psychic", HP, 48, 45, 43, 90, 42, ownerId, 20, "Insomnia")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Pound, Hypnosis, Disable, Confusion, Headbutt, Poison Gas, Psychic, Meditate, Toxic, Body Slam, Take Down, Double-Edge, Seismic Toss, Rage, Thunder Wave, Mimic, Double Team, Reflect, Bide, Metronome, Skull Bash, Dream Eater, Rest, Psywave, Substitute";

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
        if (Level >= 26) {
            using (var context = new DatabaseContext())
            {
                var hypno = new Hypno(this);
                hypno.MaxHealth = hypno.HealthOverride;
                hypno.EvolveLevelUp(Level-1);

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                // Add skills for the evolved Pokemon
                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(hypno);
                foreach (var skill in hypno.Skills)
                {
                    context.Skills.Add(skill);
                }
                
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Drowzee to a Hypno!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }
    
    public override async Task GodEvolve(ClientSession session)
    {
        using (var context = new DatabaseContext())
        {
            var hypno = new Hypno(this);
            hypno.MaxHealth = hypno.HealthOverride;
            hypno.EvolveLevelUp(Level-1);

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            // Add skills for the evolved Pokemon
            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(hypno);
            foreach (var skill in hypno.Skills)
            {
                context.Skills.Add(skill);
            }
            
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Drowzee to a Hypno!");
    }

    public override float calculateDamage(float SkillDamage)
    {
        return SkillDamage;
    }
}
