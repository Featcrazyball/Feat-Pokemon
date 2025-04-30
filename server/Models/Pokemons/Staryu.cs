namespace PokemonPocket;

public class Staryu : PokemonMaster
{
    public string? Nickname {get;set;}

    private Staryu() { } //For EF Core
    public Staryu(string nickname, string ownerId) 
    : base("Staryu", "Water", 30, 45, 55, 70, 55, 85, ownerId, 20, "Illuminate")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}