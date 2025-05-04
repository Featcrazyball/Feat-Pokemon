namespace PokemonPocket;

public class Electabuzz : PokemonMaster
{
    public string? Nickname {get;set;}

    private Electabuzz() { } //For EF Core
    public Electabuzz(string nickname, string ownerId) 
    : base("Electabuzz", "Electric", 65, 83, 57, 95, 85, 105, ownerId, 30, "Static")
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