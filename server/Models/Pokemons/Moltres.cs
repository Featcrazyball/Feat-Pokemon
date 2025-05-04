namespace PokemonPocket;

public class Moltres : PokemonMaster
{
    public string? Nickname {get;set;}

    private Moltres() { } //For EF Core
    public Moltres(string nickname, string ownerId) 
    : base("Moltres", "Fire/Flying", 90, 100, 90, 125, 85, 90, ownerId, 30, "Flame Body")
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