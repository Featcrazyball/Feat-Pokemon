using Database;
using Server;
namespace PokemonPocket;

public class Machop : PokemonMaster
{
    public override float HealthOverride {get;set;} = 70;
    public override string? Requirements { get; set; } = "Level 28";
    public override string? EvolvesTo {get;set;} = "Machoke";
    private Machop() { } //For EF Core
    public Machop(string nickname, string ownerId) 
    : base("Machop", "Fighting", 70, 80, 50, 35, 35, 35, ownerId, 10, "Guts")
    {
        Nickname = nickname;
        SkillPool = "Karate Chop, Low Kick, Leer, Focus Energy, Seismic Toss, Submission, Strength, Earthquake, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Machop(float HP, string nickname, string ownerId, int exp)
    : base("Machop", "Fighting", HP, 80, 50, 35, 35, 35, ownerId, 10, "Guts")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Karate Chop, Low Kick, Leer, Focus Energy, Seismic Toss, Submission, Strength, Earthquake, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
        if (Level >= 28) {
            using (var context = new DatabaseContext())
            {
                var machoke = new Machoke(this);
                machoke.MaxHealth = machoke.HealthOverride;
                machoke.EvolveLevelUp(Level-1); 

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(machoke);
                foreach (var skill in machoke.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Machop to a Machoke!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}
