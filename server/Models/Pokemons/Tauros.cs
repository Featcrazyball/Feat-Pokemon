namespace PokemonPocket;

public class Tauros : PokemonMaster
{
    public string? Nickname {get;set;}

    private Tauros() { } //For EF Core
    public Tauros(string nickname, string ownerId) 
    : base("Tauros", "Normal", 75, 100, 95, 40, 70, 110, ownerId, 30, "Intimidate")
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