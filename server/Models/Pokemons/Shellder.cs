namespace PokemonPocket;

public class Shellder : PokemonMaster
{
    public string? Nickname {get;set;}

    private Shellder() { } //For EF Core
    public Shellder(string nickname, string ownerId) 
    : base("Shellder", "Water", 30, 65, 100, 45, 25, 40, ownerId, 15, "Shell Armor")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}