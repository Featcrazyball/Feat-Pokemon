namespace PokemonPocket;

public class Charmeleon : PokemonMaster
{
    public string? Nickname {get;set;}

    private Charmeleon() { } //For EF Core
    public Charmeleon(string nickname, string ownerId) 
    : base("Charmeleon", "Fire", 58, 64, 58, 80, 65, 80, ownerId, 25, "Fire Burst")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}