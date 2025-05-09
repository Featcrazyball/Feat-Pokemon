using Database;
using Server;
namespace PokemonPocket;

public class Meowth : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 28";
    private Meowth() { } //For EF Core
    public Meowth(string nickname, string ownerId) 
    : base("Meowth", "Normal", 40, 45, 35, 40, 40, 90, ownerId, 10, "Pickup")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 28) {
            using (var context = new DatabaseContext())
            {
                var persian = new Persian(this);
                persian.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(persian);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Meowth to a Persian!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}