using Server;
namespace PokemonPocket;

public class Electabuzz : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Electabuzz() { } //For EF Core
    public Electabuzz(string nickname, string ownerId) 
    : base("Electabuzz", "Electric", 65, 83, 57, 95, 85, 105, ownerId, 30, "Static")
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