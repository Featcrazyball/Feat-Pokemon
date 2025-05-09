using Server;
namespace PokemonPocket;

public class Farfetch : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Farfetch() { } //For EF Core
    public Farfetch(string nickname, string ownerId) 
    : base("Farfetch", "Normal/Flying", 52, 90, 55, 58, 62, 60, ownerId, 20, "Keen Eye")
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