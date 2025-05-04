namespace PokemonPocket;

public class Lapras : PokemonMaster
{
    public string? Nickname {get;set;}

    private Lapras() { } //For EF Core
    public Lapras(string nickname, string ownerId) 
    : base("Lapras", "Water/Ice", 130, 85, 80, 85, 95, 60, ownerId, 30, "Water Absorb")
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