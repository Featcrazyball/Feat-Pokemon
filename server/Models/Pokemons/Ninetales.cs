namespace PokemonPocket;

public class Ninetales : PokemonMaster
{
    public string? Nickname {get;set;}

    private Ninetales() { } //For EF Core
    public Ninetales(string nickname, string ownerId) 
    : base("Ninetales", "Fire", 73, 76, 75, 81, 100, 100, ownerId, 20, "Flash Fire")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}