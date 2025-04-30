namespace PokemonPocket;

public class Krabby : PokemonMaster
{
    public string? Nickname {get;set;}

    private Krabby() { } //For EF Core
    public Krabby(string nickname, string ownerId) 
    : base("Krabby", "Water", 30, 105, 90, 25, 25, 50, ownerId, 10, "Hyper Cutter")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}