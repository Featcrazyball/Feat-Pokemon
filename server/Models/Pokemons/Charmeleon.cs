using Database;
using Server;
namespace PokemonPocket;

public class Charmeleon : PokemonMaster
{
    private Charmeleon() { } //For EF Core
    public Charmeleon(string nickname, string ownerId) 
    : base("Charmeleon", "Fire", 58, 64, 58, 80, 65, 80, ownerId, 25, "Fire Burst")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public Charmeleon(Charmander charm)
    : base("Charmeleon", "Fire", 58, 64, 58, 80, 65, 80, charm.OwnerId ?? "Unknown", 25, "Fire Burst")
    {
        Id = charm.Id;
        Level = 1;
        Nickname = charm.Nickname;
        Experience = charm.Experience;
        HpIV = charm.HpIV;
        AttackIV = charm.AttackIV;
        SpecialAttackIV = charm.SpecialAttackIV;
        DefenseIV = charm.DefenseIV;
        SpecialDefenseIV = charm.SpecialDefenseIV;
        SpeedIV = charm.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 36) {
            using (var context = new DatabaseContext())
            {
                var charizard = new Charizard(this);
                charizard.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(charizard);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Charmeleon to a Charizard!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}