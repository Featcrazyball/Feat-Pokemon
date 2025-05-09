using Server;
namespace PokemonPocket;

public class Flareon : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Flareon() { } //For EF Core
    public Flareon(string nickname, string ownerId) 
    : base("Flareon", "Fire", 65, 130, 60, 95, 110, 65, ownerId, 20, "Flash Fire")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public Flareon(Eevee eevee)
    : base("Flareon", "Fire", 65, 130, 60, 95, 110, 65, eevee.OwnerId?? "Unknown", 20, "Flash Fire")
    {
        Id = eevee.Id;
        Level = 1;
        Nickname = eevee.Nickname;
        Experience = eevee.Experience;
        HpIV = eevee.HpIV;
        AttackIV = eevee.AttackIV;
        SpecialAttackIV = eevee.SpecialAttackIV;
        DefenseIV = eevee.DefenseIV;
        SpecialDefenseIV = eevee.SpecialDefenseIV;
        SpeedIV = eevee.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;

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