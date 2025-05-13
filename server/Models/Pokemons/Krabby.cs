using Database;
using Server;
namespace PokemonPocket;

public class Krabby : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 28";
    public override string? EvolvesTo {get;set;} = "Kingler";
    private Krabby() { } //For EF Core
    public Krabby(string nickname, string ownerId) 
    : base("Krabby", "Water", 30, 105, 90, 25, 25, 50, ownerId, 10, "Hyper Cutter")
    {
        Nickname = nickname;
        SkillPool = "Bubble, Leer, Guillotine, Stomp, Crabhammer, Harden, Toxic, Body Slam, Take Down, Double-Edge, Bubble Beam, Ice Beam, Blizzard, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Surf, Strength";

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
                var kingler = new Kingler(this);
                kingler.EvolveLevelUp(Level-1); 

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(kingler);
                foreach (var skill in kingler.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Krabby to a Kingler!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}