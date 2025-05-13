using Database;
using Server;
namespace PokemonPocket;

public class Charmeleon : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 36";
    public override string? EvolvesTo {get;set;} = "Charizard";
    private Charmeleon() { } //For EF Core
    public Charmeleon(string nickname, string ownerId) 
    : base("Charmeleon", "Fire", 58, 64, 58, 80, 65, 80, ownerId, 25, "Fire Burst")
    {
        Nickname = nickname;
        SkillPool = "Scratch, Growl, Ember, Leer, Rage, Slash, Flamethrower, Fire Spin, Toxic, Body Slam, Take Down, Double-Edge, Submission, Seismic Toss, Counter, Dragon Rage, Dig, Mimic, Double Team, Reflect, Bide, Fire Blast, Swift, Skull Bash, Rest, Substitute";


        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Charmeleon(Charmander charm)
    : base("Charmeleon", "Fire", 58, 64, 58, 80, 65, 80, charm.OwnerId ?? "Unknown", 25, "Fire Burst")
    {
        Id = charm.Id;
        Level = 1;
        Nickname = charm.Nickname;
        Experience = charm.Experience;
        HpIV = charm.HpIV;
        AttackIV = charm.AttackIV;
        SpecialAttackIV = charm.SpecialAttackIV;
        DefenseIV = charm.DefenseIV;
        SpecialDefenseIV = charm.SpecialDefenseIV;
        SpeedIV = charm.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Scratch, Growl, Ember, Leer, Rage, Slash, Flamethrower, Fire Spin, Toxic, Body Slam, Take Down, Double-Edge, Submission, Seismic Toss, Counter, Dragon Rage, Dig, Mimic, Double Team, Reflect, Bide, Fire Blast, Swift, Skull Bash, Rest, Substitute";

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
        if (Level >= 36) {  // Charmeleon evolves at level 36
            using (var context = new DatabaseContext())
            {
                var charizard = new Charizard(this);
                charizard.EvolveLevelUp(Level-1);

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                // Add the evolved Pokemon to the context
                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(charizard);
                
                // Add all skills for the evolved Pokemon
                foreach (var skill in charizard.Skills)
                {
                    context.Skills.Add(skill);
                }
                
                // Save all changes in a single transaction
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Charmeleon to a Charizard!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}