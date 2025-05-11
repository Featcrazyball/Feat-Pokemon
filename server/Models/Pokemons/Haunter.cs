using Database;
using Server;
namespace PokemonPocket;

public class Haunter : PokemonMaster
{
    public override string? Requirements { get; set; } = "Trade";
    private Haunter() { } //For EF Core
    public Haunter(string nickname, string ownerId) 
    : base("Haunter", "Ghost/Poison", 45, 50, 45, 115, 55, 95, ownerId, 25, "Levitate")
    {
        Nickname = nickname;
        SkillPool = "Lick, Confuse Ray, Night Shade, Hypnosis, Dream Eater, Toxic, Psychic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Haunter(Gastly gastly)
    : base("Haunter", "Ghost/Poison", 45, 50, 45, 115, 55, 95, gastly.OwnerId ?? "Unknown", 25, "Levitate")
    {
        Id = gastly.Id;
        Level = 1;
        Nickname = gastly.Nickname;
        Experience = gastly.Experience;
        HpIV = gastly.HpIV;
        AttackIV = gastly.AttackIV;
        SpecialAttackIV = gastly.SpecialAttackIV;
        DefenseIV = gastly.DefenseIV;
        SpecialDefenseIV = gastly.SpecialDefenseIV;
        SpeedIV = gastly.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Lick, Confuse Ray, Night Shade, Hypnosis, Dream Eater, Toxic, Psychic, Mimic, Double Team, Reflect, Bide, Rest, Substitute";


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
        if (Level >= 1) {
            using (var context = new DatabaseContext())
            {
                var gengar = new Gengar(this);
                gengar.EvolveLevelUp(Level-1); 

                context.PokemonMaster.Add(gengar);
                foreach (var skill in gengar.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Haunter to a Gengar!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}