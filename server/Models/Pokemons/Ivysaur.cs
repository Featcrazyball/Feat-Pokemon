using Database;
namespace PokemonPocket;

public class Ivysaur : PokemonMaster
{
    public string? Nickname {get;set;}

    private Ivysaur() { } //For EF Core
    public Ivysaur(string nickname, string ownerId) 
    : base("Ivysaur", "Grass/Poison", 60, 62, 63, 80, 80, 60, ownerId, 20, "Water Burst")
    {
        Nickname = nickname;
    }

    public Ivysaur(Bulbasaur bulbasaur)
    : base("Ivysaur", "Grass/Poison", 60, 62, 63, 80, 80, 60, bulbasaur.OwnerId ?? "Unknown", 20, "Water Burst")
    {
        Id = bulbasaur.Id;
        Nickname = bulbasaur.Nickname;
        Level = 1;
        Experience = bulbasaur.Experience;
        HpIV = bulbasaur.HpIV;
        AttackIV = bulbasaur.AttackIV;
        SpecialAttackIV = bulbasaur.SpecialAttackIV;
        DefenseIV = bulbasaur.DefenseIV;
        SpecialDefenseIV = bulbasaur.SpecialDefenseIV;
        SpeedIV = bulbasaur.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
    }

    public override void Evolve()
    {
        if (Level >= 32) {
            using (var context = new DatabaseContext())
            {
                var venusaur = new Venusaur(this);
                venusaur.EvolveLevelUp(Level-1);

                // Remove previous and add new Pokemon
                context.PokemonMaster.Add(venusaur);
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            Console.WriteLine($"{Nickname} has evolved from a Ivysaur to a Venusaur!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}