using Database;
using Server;
namespace PokemonPocket;

public class Dragonair : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 55";
    private Dragonair() { } //For EF Core
    public Dragonair(string nickname, string ownerId) 
    : base("Dragonair", "Dragon", 61, 84, 65, 70, 70, 70, ownerId, 30, "Shed Skin")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public Dragonair(Dratini dratini)
    : base("Dragonair", "Dragon", 61, 84, 65, 70, 70, 70, dratini.OwnerId?? "Unknown", 30, "Shed Skin")
    {
        Id = dratini.Id;
        Level = 1;
        Nickname = dratini.Nickname;
        Experience = dratini.Experience;
        HpIV = dratini.HpIV;
        AttackIV = dratini.AttackIV;
        SpecialAttackIV = dratini.SpecialAttackIV;
        DefenseIV = dratini.DefenseIV;
        SpecialDefenseIV = dratini.SpecialDefenseIV;
        SpeedIV = dratini.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 55) {
            using (var context = new DatabaseContext())
            {
                var dragonite = new Dragonite(this);
                dragonite.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(dragonite);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Dragonair to a Dragonite!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}