namespace PokemonPocket;

public class Clefairy : PokemonMaster
{
    public string? Nickname {get;set;}

    private Clefairy() { } //For EF Core
    public Clefairy(string nickname, string ownerId) 
    : base("Clefairy", "Fairy", 70, 45, 48, 60, 65, 35, ownerId, 10, "Cute Charm")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}