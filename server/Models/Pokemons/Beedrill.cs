using Server;
using Database;
namespace PokemonPocket;

public class Beedrill : PokemonMaster
{
    public override string? Requirements { get; set; } = "Unevolvable";
    private Beedrill() { } //For EF Core
    public Beedrill(string nickname, string ownerId) 
    : base("Beedrill", "Bug/Poison", 65, 90, 40, 45, 80, 75, ownerId, 20, "Swarm")
    {
        Nickname = nickname;
        SkillPool = "Fury Attack, Focus Energy, Twinneedle, Rage, Agility, Toxic, Take Down, Double-Edge, Hyper Beam, Mimic, Skull Bash, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Beedrill(Kakuna kakuna)
    : base("Beedrill", "Bug/Poison", 65, 90, 40, 45, 80, 75, kakuna.OwnerId ?? "Unknown", 20, "Swarm")
    {
        Id = kakuna.Id;
        Level = 1;
        Nickname = kakuna.Nickname;
        Experience = kakuna.Experience;
        HpIV = kakuna.HpIV;
        AttackIV = kakuna.AttackIV;
        SpecialAttackIV = kakuna.SpecialAttackIV;
        DefenseIV = kakuna.DefenseIV;
        SpecialDefenseIV = kakuna.SpecialDefenseIV;
        SpeedIV = kakuna.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Fury Attack, Focus Energy, Twinneedle, Rage, Agility, Toxic, Take Down, Double-Edge, Hyper Beam, Mimic, Skull Bash, Rest, Substitute";

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
        await session.SendMessageAsync($"{Nickname} is already at its final form!");
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}