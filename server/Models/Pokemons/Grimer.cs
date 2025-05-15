using Database;
using Server;
namespace PokemonPocket;

public class Grimer : PokemonMaster
{
    public override float HealthOverride {get;set;} = 80;
    public override string? Requirements { get; set; } = "Level 38";
    public override string? EvolvesTo {get;set;} = "Muk";
    private Grimer() { } //For EF Core
    public Grimer(string nickname, string ownerId) 
    : base("Grimer", "Poison", 80, 80, 50, 40, 50, 25, ownerId, 15, "Poison Touch")
    {
        Nickname = nickname;
        SkillPool = "Pound, Disable, Poison Gas, Minimize, Sludge, Harden, Screech, Acid Armor, Toxic, Body Slam, Take Down, Double-Edge, Rage, Mimic, Double Team, Reflect, Bide, Fire Blast, Rest, Substitute";

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
        if (Level >= 38) {
            using (var context = new DatabaseContext())
            {
                var muk = new Muk(this);
                muk.EvolveLevelUp(Level-1); 

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(muk);
                foreach (var skill in muk.Skills)
                {
                    context.Skills.Add(skill);
                }

                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Grimer to a Muk!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}
