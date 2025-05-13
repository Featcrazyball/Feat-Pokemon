using Server;
using Database;
namespace PokemonPocket;

public class Zapdos : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Zapdos() { } //For EF Core
    public Zapdos(string nickname, string ownerId) 
    : base("Zapdos", "Electric/Flying", 90, 90, 85, 125, 90, 100, ownerId, 30, "Pressure")
    {
        Nickname = nickname;
        SkillPool = "Peck, Thunder Shock, Thunder Wave, Agility, Drill Peck, Thunder, Toxic, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Fly";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public override async Task Evolve(ClientSession session)
    {
        await session.SendMessageAsync($"{Nickname == "None" ? Name : Nickname} is already at its final evolution stage.");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}