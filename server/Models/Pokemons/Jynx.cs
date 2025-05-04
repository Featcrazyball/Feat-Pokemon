namespace PokemonPocket;

public class Jynx : PokemonMaster
{
    public string? Nickname {get;set;}

    private Jynx() { } //For EF Core
    public Jynx(string nickname, string ownerId) 
    : base("Jynx", "Ice/Psychic", 65, 50, 35, 115, 95, 95, ownerId, 30, "Oblivious")
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