using Database;
using Server;
namespace PokemonPocket;

public class Magikarp : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 16";
    private Magikarp() { } //For EF Core
    public Magikarp(string nickname, string ownerId) 
    : base("Magikarp", "Water", 20, 10, 55, 15, 20, 80, ownerId, 5, "Splash")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 16) {
            using (var context = new DatabaseContext())
            {
                var gyarados = new Gyarados(this);
                gyarados.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(gyarados);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Magikarp to a Gyarados!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}