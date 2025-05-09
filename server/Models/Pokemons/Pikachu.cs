using Database;
using Server;
namespace PokemonPocket;

public class Pikachu : PokemonMaster
{
    public override string? Requirements { get; set; } = "1 Thunder Stone";
    private Pikachu() { } //For EF Core
    public Pikachu(string nickname, string ownerId) 
    : base("Pikachu", "Electric", 35, 55, 40, 50, 50, 90, ownerId, 30, "Lightning Bolt")
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
            var item = context.Items.FirstOrDefault(i => i.Name == "Thunder Stone" && i.OwnerId == OwnerId);
            if (item != null) {
                context.Items.Remove(item);
            } else {
                await session.SendMessageAsync($"{Nickname} needs a Thunderstone to evolve!");
                return;
            }

            var raichu = new Raichu(this);
            raichu.EvolveLevelUp(Level-1); // Level up to current level

            // Remove previous and add new Pokemon
            context.PokemonMaster.Add(raichu);
            context.PokemonMaster.Remove(this);
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{Nickname} has evolved from a Pikachu to a Raichu!");
    }

    public override float calculateDamage(float SkillDamage) {
        return 3*SkillDamage;
    }
}