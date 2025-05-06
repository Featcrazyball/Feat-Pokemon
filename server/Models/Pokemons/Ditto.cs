using Server;
namespace PokemonPocket;

public class Ditto : PokemonMaster
{
    private Ditto() { } //For EF Core
    public Ditto(string nickname, string ownerId) 
    : base("Ditto", "Normal", 48, 48, 48, 48, 48, 48, ownerId, 20, "Limber")
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