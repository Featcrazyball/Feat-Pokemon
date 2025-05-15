using Database;
using Server;
namespace PokemonPocket;

public class Growlithe : PokemonMaster
{
    public override float HealthOverride {get;set;} = 55;
    public override string? Requirements { get; set; } = "Fire Stone";
    public override string? EvolvesTo {get;set;} = "Arcanine";
    private Growlithe() { } //For EF Core
    public Growlithe(string nickname, string ownerId) 
    : base("Growlithe", "Fire", 55, 70, 45, 70, 50, 60, ownerId, 10, "Intimidate")
    {
        Nickname = nickname;
        SkillPool = "Bite, Roar, Ember, Leer, Take Down, Agility, Flamethrower, Toxic, Body Slam, Take Down, Double-Edge, Rage, Mimic, Double Team, Reflect, Bide, Fire Blast, Swift, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Growlithe(float HP, string nickname, string ownerId, int exp)
    : base("Growlithe", "Fire", HP, 70, 45, 70, 50, 60, ownerId, 10, "Intimidate")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Bite, Roar, Ember, Leer, Take Down, Agility, Flamethrower, Toxic, Body Slam, Take Down, Double-Edge, Rage, Mimic, Double Team, Reflect, Bide, Fire Blast, Swift, Rest, Substitute";

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

            var arcanine = new Arcanine(this);
                arcanine.MaxHealth = arcanine.HealthOverride;
            arcanine.EvolveLevelUp(Level-1); // Level up to current level

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(arcanine);
            foreach (var skill in arcanine.Skills)
            {
                context.Skills.Add(skill);
            }

            context.SaveChanges();
        }
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Growlithe to a Arcanine!");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}
