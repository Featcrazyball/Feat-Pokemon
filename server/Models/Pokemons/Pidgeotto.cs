namespace PokemonPocket;

public class Pidgeotto : PokemonMaster
{
    public string? Nickname {get;set;}

    private Pidgeotto() { } //For EF Core
    public Pidgeotto(string nickname, string ownerId) 
    : base("Pidgeotto", "Normal/Flying", 63, 60, 55, 50, 50, 71, ownerId, 25, "Gust")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}