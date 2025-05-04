namespace PokemonPocket;

public class Ditto : PokemonMaster
{
    public string? Nickname {get;set;}

    private Ditto() { } //For EF Core
    public Ditto(string nickname, string ownerId) 
    : base("Ditto", "Normal", 48, 48, 48, 48, 48, 48, ownerId, 20, "Limber")
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