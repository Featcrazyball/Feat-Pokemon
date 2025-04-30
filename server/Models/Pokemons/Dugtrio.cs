namespace PokemonPocket;

public class Dugtrio : PokemonMaster
{
    public string? Nickname {get;set;}

    private Dugtrio() { } //For EF Core
    public Dugtrio(string nickname, string ownerId) 
    : base("Dugtrio", "Ground", 35, 100, 50, 50, 70, 120, ownerId, 26, "Sand Veil")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}