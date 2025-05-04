using Database;
namespace PokemonPocket;

public class Gloom : PokemonMaster
{
    public string? Nickname {get;set;}

    private Gloom() { } //For EF Core
    public Gloom(string nickname, string ownerId) 
    : base("Gloom", "Grass/Poison", 60, 65, 70, 85, 75, 40, ownerId, 20, "Chlorophyll")
    {
        Nickname = nickname;
    }

    public Gloom(Oddish oddish)
    : base("Gloom", "Grass/Poison", 60, 65, 70, 85, 75, 40, oddish.OwnerId ?? "Unknown", 20, "Chlorophyll")
    {
        Id = oddish.Id;
        Level = 1;
        Nickname = oddish.Nickname;
        Experience = oddish.Experience;
        HpIV = oddish.HpIV;
        AttackIV = oddish.AttackIV;
        SpecialAttackIV = oddish.SpecialAttackIV;
        DefenseIV = oddish.DefenseIV;
        SpecialDefenseIV = oddish.SpecialDefenseIV;
        SpeedIV = oddish.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
    }

    public override void Evolve()
    {
        using (var context = new DatabaseContext())
        {
            var item = context.Items.FirstOrDefault(i => i.Name == "Leaf Stone" && i.OwnerId == OwnerId);
            if (item != null) {
                context.Items.Remove(item);
            } else {
                Console.WriteLine($"{Nickname} needs a Leaf Stone to evolve!");
                return;
            }

            var Vileplume = new Vileplume(this);
            Vileplume.EvolveLevelUp(Level-1); // Level up to current level

            // Remove previous and add new Pokemon
            context.PokemonMaster.Add(Vileplume);
            context.PokemonMaster.Remove(this);
            context.SaveChanges();
        }
        Console.WriteLine($"{Nickname} has evolved from a Gloom to a Vileplume!");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}