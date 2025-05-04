namespace PokemonPocket;

public class Mew : PokemonMaster
{
    public string? Nickname {get;set;}

    private Mew() { } //For EF Core
    public Mew(string nickname, string ownerId) 
    : base("Mew", "Psychic", 100, 100, 100, 100, 100, 100, ownerId, 30, "Synchronize")
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