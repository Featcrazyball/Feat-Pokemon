namespace PokemonPocket;

public class Seaking : PokemonMaster
{
    public string? Nickname {get;set;}

    private Seaking() { } //For EF Core
    public Seaking(string nickname, string ownerId) 
    : base("Seaking", "Water", 80, 92, 65, 65, 80, 68, ownerId, 30, "Swift Swim")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}