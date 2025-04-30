namespace PokemonPocket;

public class Alakazam : PokemonMaster
{
    public string? Nickname {get;set;}

    private Alakazam() { } //For EF Core
    public Alakazam(string nickname, string ownerId) 
    : base("Alakazam", "Psychic", 55, 50, 45, 135, 95, 120, ownerId, 20, "Synchronize")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}