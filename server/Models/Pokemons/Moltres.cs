using Server;
namespace PokemonPocket;

public class Moltres : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Moltres() { } //For EF Core
    public Moltres(string nickname, string ownerId) 
    : base("Moltres", "Fire/Flying", 90, 100, 90, 125, 85, 90, ownerId, 30, "Flame Body")
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