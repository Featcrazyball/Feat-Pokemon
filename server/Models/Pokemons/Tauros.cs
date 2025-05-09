using Server;
namespace PokemonPocket;

public class Tauros : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Tauros() { } //For EF Core
    public Tauros(string nickname, string ownerId) 
    : base("Tauros", "Normal", 75, 100, 95, 40, 70, 110, ownerId, 30, "Intimidate")
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