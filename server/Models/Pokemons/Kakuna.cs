using Database;
namespace PokemonPocket;
    
public class Kakuna : PokemonMaster
{
    public string? Nickname {get;set;}

    private Kakuna() { } //For EF Core
    public Kakuna(string nickname, string ownerId) 
    : base("Kakuna", "Bug/Poison", 45, 25, 50, 25, 25, 35, ownerId, 15, "Shed Skin")
    {
        Nickname = nickname;
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
    }

    public override void Evolve()
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
            Console.WriteLine($"{Nickname} has evolved from a Kakuna to a Beedrill!");
        } else {
            Console.WriteLine($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}