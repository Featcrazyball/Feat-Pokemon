namespace PokemonPocket;

public class Gyarados : PokemonMaster
{
    public string? Nickname {get;set;}

    private Gyarados() { } //For EF Core
    public Gyarados(string nickname, string ownerId) 
    : base("Gyarados", "Water/Ice", 95, 125, 79, 60, 100, 81, ownerId, 30, "Intimidate")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}