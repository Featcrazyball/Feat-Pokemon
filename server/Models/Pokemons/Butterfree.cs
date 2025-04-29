namespace PokemonPocket;

public class Butterfree : PokemonMaster
{
    public string? Nickname {get;set;}

    private Butterfree() { } //For EF Core
    public Butterfree(string nickname, string ownerId) 
    : base("Butterfree", "Bug/Flying", 60, 45, 50, 90, 80, 70, ownerId, 25, "Confusion")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}