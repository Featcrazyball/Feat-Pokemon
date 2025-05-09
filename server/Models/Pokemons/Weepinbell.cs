using Database;
using Server;
namespace PokemonPocket;

public class Weepinbell : PokemonMaster
{
    public override string? Requirements { get; set; } = "1 Leaf Stone";
    private Weepinbell() { } //For EF Core
    public Weepinbell(string nickname, string ownerId) 
    : base("Weepinbell", "Grass/Poison", 65, 90, 50, 85, 45, 55, ownerId, 21, "Chlorophyll")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public Weepinbell(Bellsprout bellsprout)
    : base("Weepinbell", "Grass/Poison", 65, 90, 50, 85, 45, 55, bellsprout.OwnerId ?? "Unknown", 21, "Chlorophyll")
    {
        Id = bellsprout.Id;
        Level = 1;
        Nickname = bellsprout.Nickname;
        Experience = bellsprout.Experience;
        HpIV = bellsprout.HpIV;
        AttackIV = bellsprout.AttackIV;
        SpecialAttackIV = bellsprout.SpecialAttackIV;
        DefenseIV = bellsprout.DefenseIV;
        SpecialDefenseIV = bellsprout.SpecialDefenseIV;
        SpeedIV = bellsprout.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        using (var context = new DatabaseContext())
        {
            var item = context.Items.FirstOrDefault(i => i.Name == "Leaf Stone" && i.OwnerId == OwnerId);
            if (item != null) {
                context.Items.Remove(item);
            } else {
                await session.SendMessageAsync($"{Nickname} needs a Leaf Stone to evolve!");
                return;
            }

            var victreebel = new Victreebel(this);
            victreebel.EvolveLevelUp(Level-1); // Level up to current level

            // Remove previous and add new Pokemon
            context.PokemonMaster.Add(victreebel);
            context.PokemonMaster.Remove(this);
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{Nickname} has evolved from a Weepinbell to a Victreebel!");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}