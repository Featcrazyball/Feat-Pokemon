namespace PokemonPocket;

public class Goldeen : PokemonMaster
{
    public string? Nickname {get;set;}

    private Goldeen() { } //For EF Core
    public Goldeen(string nickname, string ownerId) 
    : base("Goldeen", "Water", 45, 67, 60, 35, 50, 63, ownerId, 20, "Swift Swim")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}