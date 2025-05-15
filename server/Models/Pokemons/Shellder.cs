using Database;
using Server;
namespace PokemonPocket;

public class Shellder : PokemonMaster
{
    public override float HealthOverride {get;set;} = 30;
    public override string? Requirements { get; set; } = "1 Water Stone";
    public override string? EvolvesTo {get;set;} = "Cloyster";
    private Shellder() { } //For EF Core
    public Shellder(string nickname, string ownerId) 
    : base("Shellder", "Water", 30, 65, 100, 45, 25, 40, ownerId, 15, "Shell Armor")
    {
        Nickname = nickname;
        SkillPool = "Tackle, Withdraw, Supersonic, Clamp, Aurora Beam, Leer, Ice Beam, Surf, Body Slam, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Shellder(float HP, string nickname, string ownerId, int exp)
    : base("Shellder", "Water", HP, 65, 100, 45, 25, 40, ownerId, 15, "Shell Armor")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Tackle, Withdraw, Supersonic, Clamp, Aurora Beam, Leer, Ice Beam, Surf, Body Slam, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
            var item = context.Items.FirstOrDefault(i => i.Name == "Water Stone" && i.OwnerId == OwnerId);
            if (item != null) {
                context.Items.Remove(item);
            } else {
                await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} needs a Water Stone to evolve!");
                return;
            }

            var cloyster = new Cloyster(this);
                cloyster.MaxHealth = cloyster.HealthOverride;
            cloyster.EvolveLevelUp(Level-1); // Level up to current level

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(cloyster);
            foreach (var skill in cloyster.Skills)
            {
                context.Skills.Add(skill);
            }

            // Remove previous and add new Pokemon
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Shellder to a Cloyster!");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}
