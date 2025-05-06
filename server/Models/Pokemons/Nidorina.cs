using Database;
using Server;
namespace PokemonPocket;

public class Nidorina : PokemonMaster
{
    private Nidorina() { } //For EF Core
    public Nidorina(string nickname, string ownerId) 
    : base("Nidorina", "Poison", 70, 62, 67, 55, 55, 56, ownerId, 20, "Poison Point")
    {
        Nickname = nickname;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
            foreach (var skill in newSkills) {Skills.Add(skill);};
    }

    public Nidorina(NidoranF nidoranF)
    : base("Nidorina", "Poison", 70, 62, 67, 55, 55, 56, nidoranF.OwnerId ?? "Unknown", 20, "Poison Point")
    {
        Id = nidoranF.Id;
        Level = 1;
        Nickname = nidoranF.Nickname;
        Experience = nidoranF.Experience;
        HpIV = nidoranF.HpIV;
        AttackIV = nidoranF.AttackIV;
        SpecialAttackIV = nidoranF.SpecialAttackIV;
        DefenseIV = nidoranF.DefenseIV;
        SpecialDefenseIV = nidoranF.SpecialDefenseIV;
        SpeedIV = nidoranF.SpeedIV;
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
            var item = context.Items.FirstOrDefault(i => i.Name == "Moon Stone" && i.OwnerId == OwnerId);
            if (item != null) {
                context.Items.Remove(item);
            } else {
                await session.SendMessageAsync($"{Nickname} needs a Moon Stone to evolve!");
                return;
            }

            var nidoqueen = new Nidoqueen(this);
            nidoqueen.EvolveLevelUp(Level-1); // Level up to current level

            // Remove previous and add new Pokemon
            context.PokemonMaster.Add(nidoqueen);
            context.PokemonMaster.Remove(this);
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{Nickname} has evolved from a Nidorina to a Nidoqueen!");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}