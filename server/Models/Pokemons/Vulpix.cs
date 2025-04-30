namespace PokemonPocket;

public class Vulpix : PokemonMaster
{
    public string? Nickname {get;set;}

    private Vulpix() { } //For EF Core
    public Vulpix(string nickname, string ownerId) 
    : base("Vulpix", "Fire", 38, 41, 40, 50, 65, 65, ownerId, 10, "Flash Fire")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}