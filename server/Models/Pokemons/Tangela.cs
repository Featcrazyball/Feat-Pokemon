using Server;
namespace PokemonPocket;

public class Tangela : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Tangela() { } //For EF Core
    public Tangela(string nickname, string ownerId) 
    : base("Tangela", "Grass", 65, 55, 115, 100, 40, 60, ownerId, 20, "Chlorophyll")
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