namespace PokemonPocket;

public class Marowak : PokemonMaster
{
    public string? Nickname {get;set;}

    private Marowak() { } //For EF Core
    public Marowak(string nickname, string ownerId) 
    : base("Marowak", "Ground", 60, 80, 110, 50, 80, 45, ownerId, 20, "Lightning Rod")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}