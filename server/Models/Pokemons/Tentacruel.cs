namespace PokemonPocket;

public class Tentacruel : PokemonMaster
{
    public string? Nickname {get;set;}

    private Tentacruel() { } //For EF Core
    public Tentacruel(string nickname, string ownerId) 
    : base("Tentacruel", "Water/Poison", 80, 70, 65, 80, 120, 100, ownerId, 30, "Liquid Ooze")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}