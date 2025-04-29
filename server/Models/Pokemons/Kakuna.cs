namespace PokemonPocket;
    
public class Kakuna : PokemonMaster
{
    public string? Nickname {get;set;}

    private Kakuna() { } //For EF Core
    public Kakuna(string nickname, string ownerId) 
    : base("Kakuna", "Bug/Poison", 45, 25, 50, 25, 25, 35, ownerId, 15, "Shed Skin")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return 2*SkillDamage;
    }
}