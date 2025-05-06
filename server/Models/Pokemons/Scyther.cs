using Server;
namespace PokemonPocket;

public class Scyther : PokemonMaster
{
    private Scyther() { } //For EF Core
    public Scyther(string nickname, string ownerId) 
    : base("Scyther", "Bug/Flying", 70, 110, 80, 55, 80, 105, ownerId, 20, "Swarm")
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