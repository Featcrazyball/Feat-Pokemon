namespace PokemonPocket;

public class Kangaskhan : PokemonMaster
{
    public string? Nickname {get;set;}

    private Kangaskhan() { } //For EF Core
    public Kangaskhan(string nickname, string ownerId) 
    : base("Kangaskhan", "Normal", 105, 95, 80, 40, 80, 90, ownerId, 45, "Early Bird")
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