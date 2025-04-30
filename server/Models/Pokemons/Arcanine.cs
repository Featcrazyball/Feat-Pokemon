namespace PokemonPocket;

public class Arcanine : PokemonMaster
{
    public string? Nickname {get;set;}

    private Arcanine() { } //For EF Core
    public Arcanine(string nickname, string ownerId) 
    : base("Arcanine", "Fire", 90, 110, 80, 100, 80, 95, ownerId, 59, "Intimidate")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}