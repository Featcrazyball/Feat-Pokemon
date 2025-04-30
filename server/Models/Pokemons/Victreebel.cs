namespace PokemonPocket;

public class Victreebel : PokemonMaster
{
    public string? Nickname {get;set;}

    private Victreebel() { } //For EF Core
    public Victreebel(string nickname, string ownerId) 
    : base("Victreebel", "Grass/Poison", 80, 105, 65, 100, 70, 70, ownerId, 20, "Chlorophyll")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}