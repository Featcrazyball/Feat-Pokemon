using Database;
using Server;
namespace PokemonPocket;

public class Exeggcute : PokemonMaster
{
    public override string? Requirements { get; set; } = "1 Leaf Stone";
    private Exeggcute() { } //For EF Core
    public Exeggcute(string nickname, string ownerId) 
    : base("Exeggcute", "Grass/Psychic", 60, 40, 80, 60, 45, 40, ownerId, 20, "Chlorophyll")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        using (var context = new DatabaseContext())
        {
            var item = context.Items.FirstOrDefault(i => i.Name == "Leaf Stone" && i.OwnerId == OwnerId);
            if (item != null) {
                context.Items.Remove(item);
            } else {
                await session.SendMessageAsync($"{Nickname} needs a Leaf Stone to evolve!");
                return;
            }

            var exeggutor = new Exeggutor(this);
            exeggutor.EvolveLevelUp(Level-1); // Level up to current level

            // Remove previous and add new Pokemon
            context.PokemonMaster.Add(exeggutor);
            context.PokemonMaster.Remove(this);
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{Nickname} has evolved from a Exeggcute to a Exeggutor!");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}