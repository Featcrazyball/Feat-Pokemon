using Database;
using Server;
namespace PokemonPocket;

public class Kadabra : PokemonMaster
{
    public override string? Requirements { get; set; } = "Trade";
    private Kadabra() { } //For EF Core
    public Kadabra(string nickname, string ownerId) 
    : base("Kadabrah", "Psychic", 40, 35, 30, 120, 70, 105, ownerId, 50, "Synchronize")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public Kadabra(Abra abra) 
    : base("Kadabra", "Psychic", 40, 35, 30, 120, 70, 105, abra.OwnerId ?? "Unknown", 50, "Synchronize")
    {
        Id = abra.Id;
        Level = 1;
        Nickname = abra.Nickname;
        Experience = abra.Experience;
        HpIV = abra.HpIV;
        AttackIV = abra.AttackIV;
        SpecialAttackIV = abra.SpecialAttackIV;
        DefenseIV = abra.DefenseIV;
        SpecialDefenseIV = abra.SpecialDefenseIV;
        SpeedIV = abra.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 1) {
            using (var context = new DatabaseContext())
            {
                var alakazam = new Alakazam(this);
                alakazam.EvolveLevelUp(Level-1); // Level up to current level

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(alakazam);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Abra to a Kadabra!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}