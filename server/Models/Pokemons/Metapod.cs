using Database;
using Server;
namespace PokemonPocket;

public class Metapod : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 10";
    private Metapod() { } //For EF Core
    public Metapod(string nickname, string ownerId) 
    : base("Metapod", "Bug", 50, 20, 55, 25, 25, 30, ownerId, 25, "Harden")
    {
        Nickname = nickname;
        SkillPool = "Harden";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Metapod(Caterpie caterpie)
    : base("Metapod", "Bug", 50, 20, 55, 25, 25, 30, caterpie.OwnerId ?? "Unknown", 25, "Harden")
    {
        Id = caterpie.Id;
        Level = 1;
        Nickname = caterpie.Nickname;
        Experience = caterpie.Experience;
        HpIV = caterpie.HpIV;
        AttackIV = caterpie.AttackIV;
        SpecialAttackIV = caterpie.SpecialAttackIV;
        DefenseIV = caterpie.DefenseIV;
        SpecialDefenseIV = caterpie.SpecialDefenseIV;
        SpeedIV = caterpie.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Harden";

        using (var context = new DatabaseContext())
        {
            var newSkills = LearnSkillFromSkillPool();
            if (newSkills != null)
            {
                foreach (var skill in newSkills) 
                {
                    Skills.Add(skill);
                    context.Skills.Add(skill);
                };
                context.SaveChanges();
            }
        }
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 10) {
            using (var context = new DatabaseContext())
            {
                var butterfree = new Butterfree(this);
                butterfree.EvolveLevelUp(Level-1); // Level up to 10

                context.PokemonMaster.Add(butterfree);
                foreach (var skill in butterfree.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Metapod to a Butterfree!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}