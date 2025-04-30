namespace PokemonPocket;

public class Parasect : PokemonMaster
{
    public string? Nickname {get;set;}

    private Parasect() { } //For EF Core
    public Parasect(string nickname, string ownerId) 
    : base("Parasect", "Bug/Grass", 60, 95, 80, 60, 80, 30, ownerId, 24, "Effect Spore")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}