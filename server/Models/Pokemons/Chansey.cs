using Server;
namespace PokemonPocket;

public class Chansey : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Chansey() { } //For EF Core
    public Chansey(string nickname, string ownerId) 
    : base("Chansey", "Normal", 250, 5, 5, 35, 105, 50, ownerId, 30, "Natural Cure")
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