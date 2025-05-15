using Database;
using Server;
namespace PokemonPocket;
    
public class Kakuna : PokemonMaster
{
    public override float HealthOverride {get;set;} = 45;
    public override string? Requirements { get; set; } = "Level 7";
    public override string? EvolvesTo {get;set;} = "Beedrill";
    private Kakuna() { } //For EF Core
    public Kakuna(string nickname, string ownerId) 
    : base("Kakuna", "Bug/Poison", 45, 25, 50, 25, 25, 35, ownerId, 15, "Shed Skin")
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

    public Kakuna(float HP, string nickname, string ownerId, int exp)
    : base("Kakuna", "Bug/Poison", HP, 25, 50, 25, 25, 35, ownerId, 15, "Shed Skin")
    {
        Nickname = nickname;
        Experience = exp;
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

    public Kakuna(Weedle weedle)
    : base("Kakuna", "Bug/Poison", 100, 25, 50, 25, 25, 35, weedle.OwnerId ?? "Unknown", 15, "Shed Skin")
    {
        Id = weedle.Id;
        Level = 1;
        Nickname = weedle.Nickname;
        Experience = 0;
        HpIV = weedle.HpIV;
        AttackIV = weedle.AttackIV;
        SpecialAttackIV = weedle.SpecialAttackIV;
        DefenseIV = weedle.DefenseIV;
        SpecialDefenseIV = weedle.SpecialDefenseIV;
        SpeedIV = weedle.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
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

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 7) {
            using (var context = new DatabaseContext())
            {
                var beedrill = new Beedrill(this);
                beedrill.MaxHealth = beedrill.HealthOverride;
                beedrill.EvolveLevelUp(Level-1); // Level up to 7

                foreach (var skill in this.Skills)
                {
                    context.Skills.Remove(skill);
                }

                // Add new skills to Beedrill
                context.PokemonMaster.Remove(this);
                context.PokemonMaster.Add(beedrill);
                foreach (var skill in beedrill.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from a Kakuna to a Beedrill!");
        } else {
            await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}
