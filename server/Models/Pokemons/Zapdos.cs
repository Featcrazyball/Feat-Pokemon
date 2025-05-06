using Server;
namespace PokemonPocket;

public class Zapdos : PokemonMaster
{
    private Zapdos() { } //For EF Core
    public Zapdos(string nickname, string ownerId) 
    : base("Zapdos", "Electric/Flying", 90, 90, 85, 125, 90, 100, ownerId, 30, "Pressure")
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