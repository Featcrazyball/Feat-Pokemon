namespace PokemonPocket;

public class Vileplume : PokemonMaster
{
    public string? Nickname {get;set;}

    private Vileplume() { } //For EF Core
    public Vileplume(string nickname, string ownerId) 
    : base("Vileplume", "Grass/Poison", 75, 80, 85, 110, 90, 50, ownerId, 20, "Effect Spore")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}