using Server;
namespace PokemonPocket;

public class Jynx : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Jynx() { } //For EF Core
    public Jynx(string nickname, string ownerId) 
    : base("Jynx", "Ice/Psychic", 65, 50, 35, 115, 95, 95, ownerId, 30, "Oblivious")
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