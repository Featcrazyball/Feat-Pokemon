namespace PokemonPocket;

public class Gloom : PokemonMaster
{
    public string? Nickname {get;set;}

    private Gloom() { } //For EF Core
    public Gloom(string nickname, string ownerId) 
    : base("Gloom", "Grass/Poison", 60, 65, 70, 85, 75, 40, ownerId, 20, "Chlorophyll")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}