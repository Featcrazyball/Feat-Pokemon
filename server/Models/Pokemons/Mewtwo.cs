using System.Data.SQLite;
using Server;
namespace PokemonPocket;

public class Mewtwo : PokemonMaster
{
    private Mewtwo() { } //For EF Core
    public Mewtwo(string nickname, string ownerId) 
    : base("Mewtwo", "Psychic", 106, 110, 90, 154, 90, 130, ownerId, 70, "Pressure")
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