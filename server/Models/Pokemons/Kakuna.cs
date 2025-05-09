using Database;
using Server;
namespace PokemonPocket;
    
public class Kakuna : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 7";
    private Kakuna() { } //For EF Core
    public Kakuna(string nickname, string ownerId) 
    : base("Kakuna", "Bug/Poison", 45, 25, 50, 25, 25, 35, ownerId, 15, "Shed Skin")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public Kakuna(Weedle weedle)
    : base("Kakuna", "Bug/Poison", 45, 25, 50, 25, 25, 35, weedle.OwnerId ?? "Unknown", 15, "Shed Skin")
    {
        Id = weedle.Id;
        Level = 1;
        Nickname = weedle.Nickname;
        Experience = weedle.Experience;
        HpIV = weedle.HpIV;
        AttackIV = weedle.AttackIV;
        SpecialAttackIV = weedle.SpecialAttackIV;
        DefenseIV = weedle.DefenseIV;
        SpecialDefenseIV = weedle.SpecialDefenseIV;
        SpeedIV = weedle.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 7) {
            using (var context = new DatabaseContext())
            {
                var beedrill = new Beedrill(this);
                beedrill.EvolveLevelUp(Level-1); // Level up to 7

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(beedrill);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a Kakuna to a Beedrill!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}