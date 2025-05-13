using Database;
using Server;
namespace PokemonPocket;

public class Geodude : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 25";
    public override string? EvolvesTo {get;set;} = "Graveler";
    private Geodude() { } //For EF Core
    public Geodude(string nickname, string ownerId) 
    : base("Geodude", "Rock/Ground", 40, 80, 100, 30, 30, 20, ownerId, 10, "Sturdy")
    {
        Nickname = nickname;
        SkillPool = "Tackle, Defense Curl, Rock Throw, Self-Destruct, Harden, Earthquake, Explosion, Toxic, Body Slam, Take Down, Double-Edge, Seismic Toss, Rage, Mimic, Double Team, Reflect, Bide, Fire Blast, Rest, Substitute";

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
                var graveler = new Graveler(this);
                graveler.EvolveLevelUp(Level-1); 

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                // Add skills for the evolved Pokemon
                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(graveler);
                foreach (var skill in graveler.Skills)
                {
                    context.Skills.Add(skill);
                }

                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Geodude to a Graveler!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}