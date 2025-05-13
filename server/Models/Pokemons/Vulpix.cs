using Database;
using Server;
namespace PokemonPocket;

public class Vulpix : PokemonMaster
{
    public override string? Requirements { get; set; } = "1 Fire Stone";
    public override string? EvolvesTo {get;set;} = "Ninetales";
    private Vulpix() { } //For EF Core
    public Vulpix(string nickname, string ownerId) 
    : base("Vulpix", "Fire", 38, 41, 40, 50, 65, 65, ownerId, 10, "Flash Fire")
    {
        Nickname = nickname;
        SkillPool = "Ember, Tail Whip, Quick Attack, Roar, Confuse Ray, Flamethrower, Fire Spin, Toxic, Body Slam, Take Down, Double-Edge, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
        using (var context = new DatabaseContext())
        {
            var item = context.Items.FirstOrDefault(i => i.Name == "Fire Stone" && i.OwnerId == OwnerId);
            if (item != null) {
                context.Items.Remove(item);
            } else {
                await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} needs a Fire Stone to evolve!");
                return;
            }

            var ninetales = new Ninetales(this);
            ninetales.EvolveLevelUp(Level-1); // Level up to current level

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(ninetales);
            foreach (var skill in ninetales.Skills)
            {
                context.Skills.Add(skill);
            }

            // Remove previous and add new Pokemon
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Vulpix to a Ninetales!");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}