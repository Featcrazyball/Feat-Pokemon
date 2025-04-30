namespace PokemonPocket;

public class Kingler : PokemonMaster
{
    public string? Nickname {get;set;}

    private Kingler() { } //For EF Core
    public Kingler(string nickname, string ownerId) 
    : base("Kingler", "Water", 55, 130, 115, 50, 50, 75, ownerId, 30, "Hyper Cutter")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}