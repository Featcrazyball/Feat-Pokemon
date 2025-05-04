namespace PokemonPocket;

public class Articuno : PokemonMaster
{
    public string? Nickname {get;set;}

    private Articuno() { } //For EF Core
    public Articuno(string nickname, string ownerId) 
    : base("Articuno", "Ice/Flying", 90, 85, 100, 95, 125, 85, ownerId, 25, "Pressure")
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