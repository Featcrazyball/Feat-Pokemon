using Database;
using Server;
namespace PokemonPocket;

public class Exeggcute : PokemonMaster
{
    public override string? Requirements { get; set; } = "1 Leaf Stone";
    public override string? EvolvesTo {get;set;} = "Exeggutor";
    private Exeggcute() { } //For EF Core
    public Exeggcute(string nickname, string ownerId) 
    : base("Exeggcute", "Grass/Psychic", 60, 40, 80, 60, 45, 40, ownerId, 20, "Chlorophyll")
    {
        Nickname = nickname;
        SkillPool = "Barrage, Hypnosis, Reflect, Leech Seed, Stun Spore, Poison Powder, Solar Beam, Toxic, Psychic, Rage, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

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
            var item = context.Items.FirstOrDefault(i => i.Name == "Leaf Stone" && i.OwnerId == OwnerId);
            if (item != null) {
                context.Items.Remove(item);
            } else {
                await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} needs a Leaf Stone to evolve!");
                return;
            }

            var exeggutor = new Exeggutor(this);
            exeggutor.EvolveLevelUp(Level-1);

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            // Add skills for the evolved Pokemon
            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(exeggutor);
            foreach (var skill in exeggutor.Skills)
            {
                context.Skills.Add(skill);
            }
            
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from an Exeggcute to an Exeggutor!");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}