namespace PokemonPocket;

public class Growlithe : PokemonMaster
{
    public string? Nickname {get;set;}

    private Growlithe() { } //For EF Core
    public Growlithe(string nickname, string ownerId) 
    : base("Growlithe", "Fire", 55, 70, 45, 70, 50, 60, ownerId, 10, "Intimidate")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}