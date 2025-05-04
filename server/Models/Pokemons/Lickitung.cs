namespace PokemonPocket;

public class Lickitung : PokemonMaster
{
    public string? Nickname {get;set;}

    private Lickitung() { } //For EF Core
    public Lickitung(string nickname, string ownerId) 
    : base("Lickitung", "Normal", 90, 55, 75, 60, 75, 30, ownerId, 20, "Oblivious")
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