namespace PokemonPocket;

public class Paras : PokemonMaster
{
    public string? Nickname {get;set;}

    private Paras() { } //For EF Core
    public Paras(string nickname, string ownerId) 
    : base("Paras", "Bug/Grass", 35, 70, 55, 45, 55, 25, ownerId, 12, "Effect Spore")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}