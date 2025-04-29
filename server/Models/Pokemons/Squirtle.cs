namespace PokemonPocket;

public class Squirtle : PokemonMaster
{
    public string? Nickname {get;set;}

    private Squirtle() { } //For EF Core
    public Squirtle(string nickname, string ownerId) 
    : base("Squirtle", "Water", 44, 48, 65, 50, 64, 43, ownerId, 10, "Torrent")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}