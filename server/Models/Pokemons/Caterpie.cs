using Database;
using Server;
namespace PokemonPocket;

public class Caterpie : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 7";
    private Caterpie() { } //For EF Core
    public Caterpie(string nickname, string ownerId) 
    : base("Caterpie", "Bug", 45, 30, 35, 20, 20, 45, ownerId,  10, "Shield Dust")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 7) {
            using (var context = new DatabaseContext())
            {
                var metapod = new Metapod(this);
                metapod.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(metapod);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Caterpie to a Metapod!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}