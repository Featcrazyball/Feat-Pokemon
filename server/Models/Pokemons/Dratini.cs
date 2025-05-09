using Database;
using Server;
namespace PokemonPocket;

public class Dratini : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 30";
    private Dratini() { } //For EF Core
    public Dratini(string nickname, string ownerId) 
    : base("Dratini", "Dragon", 41, 64, 45, 50, 50, 50, ownerId, 15, "Shed Skin")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 30) {
            using (var context = new DatabaseContext())
            {
                var dragonair = new Dragonair(this);
                dragonair.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(dragonair);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Dratini to a Dragonair!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}