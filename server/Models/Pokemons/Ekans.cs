using Database;
using Server;
namespace PokemonPocket;

public class Ekans : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 22";
    public override string? EvolvesTo {get;set;} = "Arbok";
    private Ekans() { } //For EF Core
    public Ekans(string nickname, string ownerId) 
    : base("Ekans", "Poison", 35, 60, 44, 40, 54, 55, ownerId, 25, "Bite")
    {
        Nickname = nickname;
        SkillPool = "Wrap, Leer, Poison Sting, Bite, Glare, Screech, Acid, Toxic, Body Slam, Take Down, Double-Edge, Rage, Earthquake, Fissure, Dig, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
                var arbok = new Arbok(this);
                arbok.EvolveLevelUp(Level-1);

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                // Add skills for the evolved Pokemon
                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(arbok);
                foreach (var skill in arbok.Skills)
                {
                    context.Skills.Add(skill);
                }
                
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from an Ekans to an Arbok!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}