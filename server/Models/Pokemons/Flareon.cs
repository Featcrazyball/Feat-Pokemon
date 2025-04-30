namespace PokemonPocket;

public class Flareon : PokemonMaster
{
    public string? Nickname {get;set;}

    private Flareon() { } //For EF Core
    public Flareon(string nickname, string ownerId) 
    : base("Flareon", "Fire", 65, 130, 60, 95, 110, 65, ownerId, 20, "Flash Fire")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}