namespace PokemonPocket;

public class Graveler : PokemonMaster
{
    public string? Nickname {get;set;}

    private Graveler() { } //For EF Core
    public Graveler(string nickname, string ownerId) 
    : base("Graveler", "Rock/Ground", 55, 95, 115, 45, 45, 35, ownerId, 25, "Sturdy")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}