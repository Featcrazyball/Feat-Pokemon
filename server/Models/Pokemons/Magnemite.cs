namespace PokemonPocket;

public class Magnemite : PokemonMaster
{
    public string? Nickname {get;set;}

    private Magnemite() { } //For EF Core
    public Magnemite(string nickname, string ownerId) 
    : base("Magnemite", "Electric/Steel", 25, 35, 70, 95, 55, 45, ownerId, 10, "Magnet Pull")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}