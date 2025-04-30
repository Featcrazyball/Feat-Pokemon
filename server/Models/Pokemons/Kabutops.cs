namespace PokemonPocket;

public class Kabutops : PokemonMaster
{
    public string? Nickname {get;set;}

    private Kabutops() { } //For EF Core
    public Kabutops(string nickname, string ownerId) 
    : base("Kabutops", "Rock/Water", 60, 115, 105, 65, 70, 80, ownerId, 40, "Swift Swim")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}