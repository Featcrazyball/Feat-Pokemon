namespace PokemonPocket;

public class Hitmonlee : PokemonMaster
{
    public string? Nickname {get;set;}

    private Hitmonlee() { } //For EF Core
    public Hitmonlee(string nickname, string ownerId) 
    : base("Hitmonlee", "Fighting", 50, 120, 53, 35, 110, 87, ownerId, 20, "Limber")
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