using Database;
using Server;
namespace PokemonPocket;

public class Paras : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 24";
    private Paras() { } //For EF Core
    public Paras(string nickname, string ownerId) 
    : base("Paras", "Bug/Grass", 35, 70, 55, 45, 55, 25, ownerId, 12, "Effect Spore")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 24) {
            using (var context = new DatabaseContext())
            {
                var parasect = new Parasect(this);
                parasect.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(parasect);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Paras to a Parasect!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}