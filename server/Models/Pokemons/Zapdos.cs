namespace PokemonPocket;

public class Zapdos : PokemonMaster
{
    public string? Nickname {get;set;}

    private Zapdos() { } //For EF Core
    public Zapdos(string nickname, string ownerId) 
    : base("Zapdos", "Electric/Flying", 90, 90, 85, 125, 90, 100, ownerId, 30, "Pressure")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}