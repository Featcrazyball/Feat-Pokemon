using Database;
using Server;
namespace PokemonPocket;

public class Clefairy : PokemonMaster
{
    private Clefairy() { } //For EF Core
    public Clefairy(string nickname, string ownerId) 
    : base("Clefairy", "Fairy", 70, 45, 48, 60, 65, 35, ownerId, 10, "Cute Charm")
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
            var item = context.Items.FirstOrDefault(i => i.Name == "Moon Stone" && i.OwnerId == OwnerId);
            if (item != null) {
                context.Items.Remove(item);
            } else {
                await session.SendMessageAsync($"{Nickname} needs a Moon Stone to evolve!");
                return;
            }

            var clefable = new Clefable(this);
            clefable.EvolveLevelUp(Level-1); // Level up to current level

            // Remove previous and add new Pokemon
            context.PokemonMaster.Add(clefable);
            context.PokemonMaster.Remove(this);
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{Nickname} has evolved from a Clefairy to a Clefable!");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}