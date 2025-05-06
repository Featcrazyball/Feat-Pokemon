using Database;
using Server;
namespace PokemonPocket;

public class Growlithe : PokemonMaster
{
    private Growlithe() { } //For EF Core
    public Growlithe(string nickname, string ownerId) 
    : base("Growlithe", "Fire", 55, 70, 45, 70, 50, 60, ownerId, 10, "Intimidate")
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
            var item = context.Items.FirstOrDefault(i => i.Name == "Fire Stone" && i.OwnerId == OwnerId);
            if (item != null) {
                context.Items.Remove(item);
            } else {
                await session.SendMessageAsync($"{Nickname} needs a Fire Stone to evolve!");
                return;
            }

            var arcanine = new Arcanine(this);
            arcanine.EvolveLevelUp(Level-1); // Level up to current level

            // Remove previous and add new Pokemon
            context.PokemonMaster.Add(arcanine);
            context.PokemonMaster.Remove(this);
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{Nickname} has evolved from a Growlithe to a Arcanine!");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}