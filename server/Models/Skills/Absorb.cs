namespace Models;

public class Absorb : Skill
{
    private Absorb() { } // For EF Core
    public Absorb(string PokemonId) : base("Absorb", "Grass", 20, 1, 1, 25, 0, 0, "Absorb the target's HP and restore your own.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override void SkillEfect()
    {
        // Logic to absorb HP from the target and restore it to the user
        Console.WriteLine($"{Name} absorbed HP from the target!");
    }
}
