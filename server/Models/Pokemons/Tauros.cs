using Server;
using Database;
namespace PokemonPocket;

public class Tauros : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Tauros() { } //For EF Core
    public Tauros(string nickname, string ownerId) 
    : base("Tauros", "Normal", 75, 100, 95, 40, 70, 110, ownerId, 30, "Intimidate")
    {
        Nickname = nickname;
        SkillPool = "Tackle, Stomp, Tail Whip, Leer, Rage, Take Down, Earthquake, Hyper Beam, Body Slam, Double-Edge, Mimic, Double Team, Reflect, Bide, Rest, Substitute, Strength";

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
        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is already at its final evolution stage.");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}