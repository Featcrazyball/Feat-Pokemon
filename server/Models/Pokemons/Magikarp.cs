namespace PokemonPocket;

public class Magikarp : PokemonMaster
{
    public string? Nickname {get;set;}

    private Magikarp() { } //For EF Core
    public Magikarp(string nickname, string ownerId) 
    : base("Magikarp", "Water", 20, 10, 55, 15, 20, 80, ownerId, 5, "Splash")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}