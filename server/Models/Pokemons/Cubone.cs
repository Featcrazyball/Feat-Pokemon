using Database;
using Server;
namespace PokemonPocket;

public class Cubone : PokemonMaster
{
    public override float HealthOverride {get;set;} = 50;
    public override string? Requirements { get; set; } = "Level 28";
    public override string? EvolvesTo {get;set;} = "Marowak";
    private Cubone() { } //For EF Core
    public Cubone(string nickname, string ownerId) 
    : base("Cubone", "Ground", 50, 50, 95, 40, 50, 35, ownerId, 20, "Lightning Rod")
    {
        Nickname = nickname;
        SkillPool = "Bone Club, Growl, Tail Whip, Headbutt, Leer, Focus Energy, Bonemerang, Rage, Thrash, Toxic, Body Slam, Take Down, Double-Edge, Submission, Seismic Toss, Earthquake, Fissure, Dig, Mimic, Double Team, Reflect, Bide, Fire Blast, Skull Bash, Rest, Substitute";

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
                var marowak = new Marowak(this);
                marowak.EvolveLevelUp(Level-1);

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                // Add the evolved Pokemon to the context
                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(marowak);
                
                // Add all skills for the evolved Pokemon
                foreach (var skill in marowak.Skills)
                {
                    context.Skills.Add(skill);
                }
                
                // Save all changes in a single transaction
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Cubone to a Marowak!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}
