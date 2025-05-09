using Server;
namespace PokemonPocket;

public class Hitmonchan : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Hitmonchan() { } //For EF Core
    public Hitmonchan(string nickname, string ownerId) 
    : base("Hitmonchan", "Fighting", 50, 105, 79, 35, 110, 76, ownerId, 20, "Keen Eye")
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