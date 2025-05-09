using Server;
namespace PokemonPocket;

public class Magmar : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Magmar() { } //For EF Core
    public Magmar(string nickname, string ownerId) 
    : base("Magmar", "Fire", 65, 95, 57, 100, 85, 93, ownerId, 30, "Flame Body")
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