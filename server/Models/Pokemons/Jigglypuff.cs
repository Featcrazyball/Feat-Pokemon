using Database;
using Server;
namespace PokemonPocket;

public class Jigglypuff : PokemonMaster
{
    private Jigglypuff() { } //For EF Core
    public Jigglypuff(string nickname, string ownerId) 
    : base("Jigglypuff", "Normal/Fairy", 115, 45, 20, 45, 25, 25, ownerId, 20, "Cute Charm")
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

            var wigglytuff = new Wigglytuff(this);
            wigglytuff.EvolveLevelUp(Level-1); // Level up to current level

            // Remove previous and add new Pokemon
            context.PokemonMaster.Add(wigglytuff);
            context.PokemonMaster.Remove(this);
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{Nickname} has evolved from a Jigglypuff to a Wigglytuff!");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}