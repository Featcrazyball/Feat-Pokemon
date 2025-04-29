namespace PokemonPocket;

public class Blastoise : PokemonMaster
{
    public string? Nickname {get;set;}

    private Blastoise() { } //For EF Core
    public Blastoise(string nickname, string ownerId) 
    : base("Blastoise", "Water", 79, 83, 100, 85, 105, 78, ownerId, 30, "Torrent")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}