namespace PokemonPocket;

public class Pidgeot : PokemonMaster
{
    public string? Nickname {get;set;}

    private Pidgeot() { } //For EF Core
    public Pidgeot(string nickname, string ownerId) 
    : base("Pidgeot", "Normal/Flying", 83, 80, 75, 70, 70, 101, ownerId, 25, "Keen Eye")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}