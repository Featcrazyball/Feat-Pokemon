using Database;
using Server;
namespace PokemonPocket;

public class Poliwhirl : PokemonMaster
{
    public override string? Requirements { get; set; } = "1 Water Stone";
    private Poliwhirl() { } //For EF Core
    public Poliwhirl(string nickname, string ownerId) 
    : base("Poliwhirl", "Water", 65, 65, 65, 50, 50, 90, ownerId, 25, "Water Absorb")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }
    
    public Poliwhirl(Poliwag poliwag)
    : base("Poliwhirl", "Water", 65, 65, 65, 50, 50, 90, poliwag.OwnerId ?? "Unknown", 25, "Water Absorb")
    {
        Id = poliwag.Id;
        Level = 1;
        Nickname = poliwag.Nickname;
        Experience = poliwag.Experience;
        HpIV = poliwag.HpIV;
        AttackIV = poliwag.AttackIV;
        SpecialAttackIV = poliwag.SpecialAttackIV;
        DefenseIV = poliwag.DefenseIV;
        SpecialDefenseIV = poliwag.SpecialDefenseIV;
        SpeedIV = poliwag.SpeedIV;
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
            var item = context.Items.FirstOrDefault(i => i.Name == "Water Stone" && i.OwnerId == OwnerId);
            if (item != null) {
                context.Items.Remove(item);
            } else {
                await session.SendMessageAsync($"{Nickname} needs a Water Stone to evolve!");
                return;
            }

            var poliwrath = new Poliwrath(this);
            poliwrath.EvolveLevelUp(Level-1); // Level up to current level

            // Remove previous and add new Pokemon
            context.PokemonMaster.Add(poliwrath);
            context.PokemonMaster.Remove(this);
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{Nickname} has evolved from a Poliwhirl to a Poliwrath!");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}