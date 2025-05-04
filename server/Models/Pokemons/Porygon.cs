namespace PokemonPocket;

public class Porygon : PokemonMaster
{
    public string? Nickname {get;set;}

    private Porygon() { } //For EF Core
    public Porygon(string nickname, string ownerId) 
    : base("Porygon", "Normal", 65, 60, 70, 85, 75, 40, ownerId, 15, "Trace")
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