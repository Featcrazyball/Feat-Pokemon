using Server;
namespace PokemonPocket;

public class Articuno : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Articuno() { } //For EF Core
    public Articuno(string nickname, string ownerId) 
    : base("Articuno", "Ice/Flying", 90, 85, 100, 95, 125, 85, ownerId, 25, "Pressure")
    {
        Nickname = nickname;
        SkillPool = "Peck, Ice Beam, Blizzard, Agility, Mist, Toxic, Rage, Mimic, Double Team, Reflect, Bide, Swift, Sky Attack, Rest, Substitute";

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