namespace PokemonPocket;

public class Persian : PokemonMaster
{
    public string? Nickname {get;set;}

    private Persian() { } //For EF Core
    public Persian(string nickname, string ownerId) 
    : base("Persian", "Normal", 65, 70, 60, 65, 65, 115, ownerId, 34, "Limber")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}   