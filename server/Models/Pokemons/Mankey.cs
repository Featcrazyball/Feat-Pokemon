namespace PokemonPocket;

public class Mankey : PokemonMaster
{
    public string? Nickname {get;set;}

    private Mankey() { } //For EF Core
    public Mankey(string nickname, string ownerId) 
    : base("Mankey", "Fighting", 40, 80, 35, 35, 45, 70, ownerId, 14, "Vital Spirit")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}