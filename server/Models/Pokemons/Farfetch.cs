namespace PokemonPocket;

public class Farfetch : PokemonMaster
{
    public string? Nickname {get;set;}

    private Farfetch() { } //For EF Core
    public Farfetch(string nickname, string ownerId) 
    : base("Farfetch", "Normal/Flying", 52, 90, 55, 58, 62, 60, ownerId, 20, "Keen Eye")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}