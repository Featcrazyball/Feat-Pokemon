namespace PokemonPocket;

public class Kabuto : PokemonMaster
{
    public string? Nickname {get;set;}

    private Kabuto() { } //For EF Core
    public Kabuto(string nickname, string ownerId) 
    : base("Kabuto", "Rock/Water", 30, 80, 90, 55, 45, 55, ownerId, 20, "Battle Armor")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}