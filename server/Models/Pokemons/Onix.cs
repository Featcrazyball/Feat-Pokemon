namespace PokemonPocket;

public class Onix : PokemonMaster
{
    public string? Nickname {get;set;}

    private Onix() { } //For EF Core
    public Onix(string nickname, string ownerId) 
    : base("Onix", "Rock/Ground", 35, 45, 160, 30, 45, 70, ownerId, 20, "Rock Head")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}