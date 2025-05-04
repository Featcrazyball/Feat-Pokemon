namespace PokemonPocket;

public class Magmar : PokemonMaster
{
    public string? Nickname {get;set;}

    private Magmar() { } //For EF Core
    public Magmar(string nickname, string ownerId) 
    : base("Magmar", "Fire", 65, 95, 57, 100, 85, 93, ownerId, 30, "Flame Body")
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