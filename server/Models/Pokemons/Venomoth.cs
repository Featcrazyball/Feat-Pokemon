namespace PokemonPocket;

public class Venomoth : PokemonMaster
{
    public string? Nickname {get;set;}

    private Venomoth() { } //For EF Core
    public Venomoth(string nickname, string ownerId) 
    : base("Venomoth", "Bug/Poison", 70, 65, 60, 90, 75, 90, ownerId, 31, "Shield Dust")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}