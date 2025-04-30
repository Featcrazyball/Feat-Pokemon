namespace PokemonPocket;

public class Dragonite : PokemonMaster
{
    public string? Nickname {get;set;}

    private Dragonite() { } //For EF Core
    public Dragonite(string nickname, string ownerId) 
    : base("Dragonite", "Dragon", 91, 134, 95, 100, 100, 80, ownerId, 60, "Inner Focus")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}