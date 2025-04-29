namespace PokemonPocket;

public class Pidgey : PokemonMaster
{
    public string? Nickname {get;set;}

    private Pidgey() { } //For EF Core
    public Pidgey(string nickname, string ownerId) 
    : base("Pidgey", "Normal/Flying", 40, 45, 40, 35, 35, 56, ownerId, 10, "Keen Eye")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}