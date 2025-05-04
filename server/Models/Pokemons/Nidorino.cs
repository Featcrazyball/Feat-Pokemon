using Database;
namespace PokemonPocket;

public class Nidorino : PokemonMaster
{
    public string? Nickname {get;set;}

    private Nidorino() { } //For EF Core
    public Nidorino(string nickname, string ownerId) 
    : base("Nidorino", "Poison", 61, 72, 57, 55, 55, 65, ownerId, 23, "Poison Point")
    {
        Nickname = nickname;
    }

    public Nidorino(NidoranM nidoran)
    : base("Nidorino", "Poison", 61, 72, 57, 55, 55, 65, nidoran.OwnerId ?? "Unknown", 23, "Poison Point")
    {
        Id = nidoran.Id;
        Level = 1;
        Nickname = nidoran.Nickname;
        Experience = nidoran.Experience;
        HpIV = nidoran.HpIV;
        AttackIV = nidoran.AttackIV;
        SpecialAttackIV = nidoran.SpecialAttackIV;
        DefenseIV = nidoran.DefenseIV;
        SpecialDefenseIV = nidoran.SpecialDefenseIV;
        SpeedIV = nidoran.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
    }

    public override void Evolve()
    {
        using (var context = new DatabaseContext())
        {
            var item = context.Items.FirstOrDefault(i => i.Name == "Moon Stone" && i.OwnerId == OwnerId);
            if (item != null) {
                context.Items.Remove(item);
            } else {
                Console.WriteLine($"{Nickname} needs a Moon Stone to evolve!");
                return;
            }

            var nidoking = new Nidoking(this);
            nidoking.EvolveLevelUp(Level-1); // Level up to current level

            // Remove previous and add new Pokemon
            context.PokemonMaster.Add(nidoking);
            context.PokemonMaster.Remove(this);
            context.SaveChanges();
        }
        Console.WriteLine($"{Nickname} has evolved from a Nidorino to a Nidoking!");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}