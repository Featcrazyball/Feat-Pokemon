using Database;
namespace PokemonPocket;

public class Eevee : PokemonMaster
{
    public string? Nickname {get;set;}

    private Eevee() { } //For EF Core
    public Eevee(string nickname, string ownerId) 
    : base("Eevee", "Normal", 55, 55, 50, 45, 65, 55, ownerId, 25, "Run Away")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        Console.WriteLine($"Eevee has 3 Evolution options: \n[1] Vaporeon (Water Stone)\n[2] Jolteon (Thunder Stone)\n[3] Flareon (Fire Stone). \nPlease choose one to evolve into.");
        string choice = Console.ReadLine() ?? "0";
        switch (choice)
        {
            case "1" or "Vaporeon":
                using (var context = new DatabaseContext())
                {
                    var item = context.Items.FirstOrDefault(i => i.Name == "Water Stone" && i.OwnerId == OwnerId);
                    if (item != null) {
                        context.Items.Remove(item);
                    } else {
                        Console.WriteLine($"{Nickname} needs a Water Stone to evolve!");
                        return;
                    }

                    var vaporeon = new Vaporeon(this);
                    vaporeon.EvolveLevelUp(Level-1); // Level up to current level

                    // Remove previous and add new Pokemon
                    context.PokemonMaster.Add(vaporeon);
                    context.PokemonMaster.Remove(this);
                    context.SaveChanges();
                }
                Console.WriteLine($"{Nickname} has evolved from an Eevee to a Vaporeon!");
                break;
            case "2" or "Jolteon":
                using (var context = new DatabaseContext())
                {
                    var item = context.Items.FirstOrDefault(i => i.Name == "Thunder Stone" && i.OwnerId == OwnerId);
                    if (item != null) {
                        context.Items.Remove(item);
                    } else {
                        Console.WriteLine($"{Nickname} needs a Thunder Stone to evolve!");
                        return;
                    }

                    var jolteon = new Jolteon(this);
                    jolteon.EvolveLevelUp(Level-1); // Level up to current level

                    // Remove previous and add new Pokemon
                    context.PokemonMaster.Add(jolteon);
                    context.PokemonMaster.Remove(this);
                    context.SaveChanges();
                }
                Console.WriteLine($"{Nickname} has evolved from an Eevee to a Jolteon!");
                break;
            case "3" or "Flareon":
                using (var context = new DatabaseContext())
                {
                    var item = context.Items.FirstOrDefault(i => i.Name == "Fire Stone" && i.OwnerId == OwnerId);
                    if (item != null) {
                        context.Items.Remove(item);
                    } else {
                        Console.WriteLine($"{Nickname} needs a Fire Stone to evolve!");
                        return;
                    }

                    var flareon = new Flareon(this);
                    flareon.EvolveLevelUp(Level-1); // Level up to current level

                    // Remove previous and add new Pokemon
                    context.PokemonMaster.Add(flareon);
                    context.PokemonMaster.Remove(this);
                    context.SaveChanges();
                }
                Console.WriteLine($"{Nickname} has evolved from an Eevee to a Flareon!");
                break;
            default:
                Console.WriteLine("Invalid choice. Eevee remains unevolved.");
                break;
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}