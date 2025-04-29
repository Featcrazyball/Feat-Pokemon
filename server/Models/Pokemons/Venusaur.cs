namespace PokemonPocket;

public class Venusaur : PokemonMaster
{
    public string? Nickname {get;set;}

    private Venusaur() { } //For EF Core
    public Venusaur(string nickname, string ownerId) 
    : base("Venusaur", "Grass/Poison", 80, 82, 83, 100, 100, 80, ownerId, 30, "Water Burst")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}