namespace PokemonPocket;

public class Tangela : PokemonMaster
{
    public string? Nickname {get;set;}

    private Tangela() { } //For EF Core
    public Tangela(string nickname, string ownerId) 
    : base("Tangela", "Grass", 65, 55, 115, 100, 40, 60, ownerId, 20, "Chlorophyll")
    {
        Nickname = nickname;
    }

    public override void Evolve()
    {
        Console.WriteLine($"{Nickname} is already at its final evolution stage.");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}