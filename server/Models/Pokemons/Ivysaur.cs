using Database;
using Server;
namespace PokemonPocket;

public class Ivysaur : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 32";
    private Ivysaur() { } //For EF Core
    public Ivysaur(string nickname, string ownerId) 
    : base("Ivysaur", "Grass/Poison", 60, 62, 63, 80, 80, 60, ownerId, 20, "Water Burst")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public Ivysaur(Bulbasaur bulbasaur)
    : base("Ivysaur", "Grass/Poison", 60, 62, 63, 80, 80, 60, bulbasaur.OwnerId ?? "Unknown", 20, "Water Burst")
    {
        Id = bulbasaur.Id;
        Nickname = bulbasaur.Nickname;
        Level = 1;
        Experience = bulbasaur.Experience;
        HpIV = bulbasaur.HpIV;
        AttackIV = bulbasaur.AttackIV;
        SpecialAttackIV = bulbasaur.SpecialAttackIV;
        DefenseIV = bulbasaur.DefenseIV;
        SpecialDefenseIV = bulbasaur.SpecialDefenseIV;
        SpeedIV = bulbasaur.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 32) {
            using (var context = new DatabaseContext())
            {
                var venusaur = new Venusaur(this);
                venusaur.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(venusaur);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Ivysaur to a Venusaur!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}