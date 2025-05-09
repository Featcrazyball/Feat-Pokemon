using Server;
namespace PokemonPocket;

public class Lickitung : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Lickitung() { } //For EF Core
    public Lickitung(string nickname, string ownerId) 
    : base("Lickitung", "Normal", 90, 55, 75, 60, 75, 30, ownerId, 20, "Oblivious")
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