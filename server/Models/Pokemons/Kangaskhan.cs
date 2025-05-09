using Server;
namespace PokemonPocket;

public class Kangaskhan : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Kangaskhan() { } //For EF Core
    public Kangaskhan(string nickname, string ownerId) 
    : base("Kangaskhan", "Normal", 105, 95, 80, 40, 80, 90, ownerId, 45, "Early Bird")
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