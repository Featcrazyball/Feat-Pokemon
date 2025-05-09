using Server;
namespace PokemonPocket;

public class Pinsir : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Pinsir() { } //For EF Core
    public Pinsir(string nickname, string ownerId) 
    : base("Pinsir", "Bug", 65, 125, 100, 55, 70, 85, ownerId, 20, "Hyper Cutter")
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