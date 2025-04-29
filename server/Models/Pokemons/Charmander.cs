namespace PokemonPocket;
    
public class Charmander : PokemonMaster
{
    public string? Nickname {get;set;}

    private Charmander() { } //For EF Core
    public Charmander(string nickname, string ownerId) 
    : base("Charmander", "Fire", 39, 52, 43, 60, 50, 65, ownerId, 10, "Solar Power")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}