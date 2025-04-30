namespace PokemonPocket;

public class Rhydon : PokemonMaster
{
    public string? Nickname {get;set;}

    private Rhydon() { } //For EF Core
    public Rhydon(string nickname, string ownerId) 
    : base("Rhydon", "Ground/Rock", 105, 130, 120, 45, 45, 40, ownerId, 30, "Lightning Rod")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}