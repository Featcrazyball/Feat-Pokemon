using Server;
namespace PokemonPocket;

public class Raichu : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Raichu() { } //For EF Core
    public Raichu(string nickname, string ownerId) 
    : base("Raichu", "Electric", 60, 90, 55, 90, 80, 110, ownerId, 25, "Thunderbolt")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public Raichu(Pikachu pikachu)
    : base("Raichu", "Electric", 60, 90, 55, 90, 80, 110, pikachu.OwnerId ?? "Unknown", 25, "Thunderbolt")
    {
        Id = pikachu.Id;
        Level = 1;
        Nickname = pikachu.Nickname;
        Experience = pikachu.Experience;
        HpIV = pikachu.HpIV;
        AttackIV = pikachu.AttackIV;
        SpecialAttackIV = pikachu.SpecialAttackIV;
        DefenseIV = pikachu.DefenseIV;
        SpecialDefenseIV = pikachu.SpecialDefenseIV;
        SpeedIV = pikachu.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        await session.SendMessageAsync($"{Nickname} is already at its final form!");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}