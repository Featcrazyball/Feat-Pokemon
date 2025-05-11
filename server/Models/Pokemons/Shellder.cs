using Database;
using Server;
namespace PokemonPocket;

public class Shellder : PokemonMaster
{
    public override string? Requirements { get; set; } = "1 Water Stone";
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

    public override async Task Evolve(ClientSession session)
    {
        using (var context = new DatabaseContext())
        {
            var item = context.Items.FirstOrDefault(i => i.Name == "Water Stone" && i.OwnerId == OwnerId);
            if (item != null) {
                context.Items.Remove(item);
            } else {
                await session.SendMessageAsync($"{Nickname} needs a Water Stone to evolve!");
                return;
            }

            var cloyster = new Cloyster(this);
            cloyster.EvolveLevelUp(Level-1); // Level up to current level

            context.PokemonMaster.Add(cloyster);
            foreach (var skill in cloyster.Skills)
            {
                context.Skills.Add(skill);
            }

            // Remove previous and add new Pokemon
            context.PokemonMaster.Remove(this);
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{Nickname} has evolved from a Shellder to a Cloyster!");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}