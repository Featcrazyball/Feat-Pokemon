using Server;
namespace PokemonPocket;

public class MrMime : PokemonMaster
{
    private MrMime() { } //For EF Core
    public MrMime(string nickname, string ownerId) 
    : base("MrMime", "Psychic", 40, 45, 65, 100, 120, 90, ownerId, 15, "Soundproof")
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