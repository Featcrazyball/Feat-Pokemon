namespace PokemonPocket;

public class MrMime : PokemonMaster
{
    public string? Nickname {get;set;}

    private MrMime() { } //For EF Core
    public MrMime(string nickname, string ownerId) 
    : base("MrMime", "Psychic", 40, 45, 65, 100, 120, 90, ownerId, 15, "Soundproof")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}