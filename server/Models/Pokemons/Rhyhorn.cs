namespace PokemonPocket;

public class Rhyhorn : PokemonMaster
{
    public string? Nickname {get;set;}

    private Rhyhorn() { } //For EF Core
    public Rhyhorn(string nickname, string ownerId) 
    : base("Rhyhorn", "Ground/Rock", 80, 85, 95, 30, 30, 25, ownerId, 20, "Lightning Rod")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}