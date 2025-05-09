using Database;
using Server;
namespace PokemonPocket;

public class Weedle : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 7";
    private Weedle() { } //For EF Core
    public Weedle(string nickname, string ownerId) 
    : base("Weedle", "Bug/Poison", 40, 35, 30, 20, 20, 50, ownerId, 10, "Shield Dust")
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
                var kakuna = new Kakuna(this);
                kakuna.EvolveLevelUp(Level-1); // Level up to 7

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(kakuna);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Weedle to a Kakuna!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}