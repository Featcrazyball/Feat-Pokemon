namespace PokemonPocket;

public class Meowth : PokemonMaster
{
    public string? Nickname {get;set;}

    private Meowth() { } //For EF Core
    public Meowth(string nickname, string ownerId) 
    : base("Meowth", "Normal", 40, 45, 35, 40, 40, 90, ownerId, 10, "Pickup")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}