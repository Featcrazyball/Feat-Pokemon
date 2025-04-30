namespace PokemonPocket;

public class Wigglytuff : PokemonMaster
{
    public string? Nickname {get;set;}

    private Wigglytuff() { } //For EF Core
    public Wigglytuff(string nickname, string ownerId) 
    : base("Wigglytuff", "Normal/Fairy", 140, 70, 45, 85, 50, 45, ownerId, 30, "Cute Charm")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}