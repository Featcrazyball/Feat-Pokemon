using Database;
using Server;
namespace PokemonPocket;

public class Zubat : PokemonMaster
{
    public override float HealthOverride {get;set;} = 40;
    public override string? Requirements { get; set; } = "Level 22";
    public override string? EvolvesTo {get;set;} = "Golbat";
    private Zubat() { } //For EF Core
    public Zubat(string nickname, string ownerId) 
    : base("Zubat", "Poison/Flying", 40, 45, 40, 30, 40, 55, ownerId, 10, "Inner Focus")
    {
        Nickname = nickname;
        SkillPool = "Leech Life, Supersonic, Bite, Confuse Ray, Wing Attack, Haze, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
        if (Level >= 22) {
            using (var context = new DatabaseContext())
            {
                var golbat = new Golbat(this);
                golbat.EvolveLevelUp(Level-1);

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(golbat);
                
                foreach (var skill in golbat.Skills)
                {
                    context.Skills.Add(skill);
                }
                
                
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Zubat to a Golbat!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}
