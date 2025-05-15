using Database;
using Server;
namespace PokemonPocket;

public class Rattata : PokemonMaster
{
    public override float HealthOverride {get;set;} = 30;
    public override string? Requirements { get; set; } = "Level 20";
    public override string? EvolvesTo {get;set;} = "Raticate";
    private Rattata() { } //For EF Core
    public Rattata(string nickname, string ownerId) 
    : base("Rattata", "Normal", 30, 56, 35, 25, 35, 72, ownerId, 25, "Run Away")
    {
        Nickname = nickname;
        SkillPool = "Tackle, Tail Whip, Quick Attack, Hyper Fang, Focus Energy, Super Fang, Body Slam, Take Down, Double-Edge, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Rattata(float HP, string nickname, string ownerId, int exp)
    : base("Rattata", "Normal", HP, 56, 35, 25, 35, 72, ownerId, 25, "Run Away")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Tackle, Tail Whip, Quick Attack, Hyper Fang, Focus Energy, Super Fang, Body Slam, Take Down, Double-Edge, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
        if (Level >= 20) {
            using (var context = new DatabaseContext())
            {
                var ratticate = new Raticate(this);
                ratticate.MaxHealth = ratticate.HealthOverride;
                ratticate.EvolveLevelUp(Level-1); // Level up to 20

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(ratticate);
                foreach (var skill in ratticate.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Rattata to a Raticate!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}
