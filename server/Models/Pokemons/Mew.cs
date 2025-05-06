using Server;
namespace PokemonPocket;

public class Mew : PokemonMaster
{
    private Mew() { } //For EF Core
    public Mew(string nickname, string ownerId) 
    : base("Mew", "Psychic", 100, 100, 100, 100, 100, 100, ownerId, 30, "Synchronize")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        await session.SendMessageAsync($"{Nickname} is already at its final evolution stage.");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}