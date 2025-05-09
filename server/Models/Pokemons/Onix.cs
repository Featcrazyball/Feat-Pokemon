using Server;
namespace PokemonPocket;

public class Onix : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Onix() { } //For EF Core
    public Onix(string nickname, string ownerId) 
    : base("Onix", "Rock/Ground", 35, 45, 160, 30, 45, 70, ownerId, 20, "Rock Head")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        await session.SendMessageAsync($"{Nickname} has reached its final evolution stage.");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}