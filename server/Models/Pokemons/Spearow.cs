using Database;
using Server;
namespace PokemonPocket;

public class Spearow : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 20";
    private Spearow() { } //For EF Core
    public Spearow(string nickname, string ownerId) 
    : base("Spearow", "Normal/Flying", 40, 60, 30, 31, 31, 70, ownerId, 25, "Peck")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 20) {
            using (var context = new DatabaseContext())
            {
                var fearow = new Fearow(this);
                fearow.EvolveLevelUp(Level-1); // Level up to 20

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(fearow);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Spearow to a Fearow!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return 3*SkillDamage;
    }
}