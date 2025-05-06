using Server;
namespace PokemonPocket;

public class Primeape : PokemonMaster
{
    private Primeape() { } //For EF Core
    public Primeape(string nickname, string ownerId) 
    : base("Primeape", "Fighting", 65, 105, 60, 60, 70, 95, ownerId, 27, "Vital Spirit")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public Primeape(Mankey mankey)
    : base("Primeape", "Fighting", 65, 105, 60, 60, 70, 95, mankey.OwnerId ?? "Unknown", 27, "Vital Spirit")
    {
        Id = mankey.Id;
        Level = 1;
        Nickname = mankey.Nickname;
        Experience = mankey.Experience; 
        HpIV = mankey.HpIV;
        AttackIV = mankey.AttackIV;
        SpecialAttackIV = mankey.SpecialAttackIV;
        DefenseIV = mankey.DefenseIV;
        SpecialDefenseIV = mankey.SpecialDefenseIV;
        SpeedIV = mankey.SpeedIV;
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