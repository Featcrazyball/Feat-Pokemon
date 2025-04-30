namespace PokemonPocket;

public class Pinsir : PokemonMaster
{
    public string? Nickname {get;set;}

    private Pinsir() { } //For EF Core
    public Pinsir(string nickname, string ownerId) 
    : base("Pinsir", "Bug", 65, 125, 100, 55, 70, 85, ownerId, 20, "Hyper Cutter")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}