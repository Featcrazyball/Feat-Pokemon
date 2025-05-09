using Server;
namespace PokemonPocket;

public class Hitmonlee : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Hitmonlee() { } //For EF Core
    public Hitmonlee(string nickname, string ownerId) 
    : base("Hitmonlee", "Fighting", 50, 120, 53, 35, 110, 87, ownerId, 20, "Limber")
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