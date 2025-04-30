namespace PokemonPocket;

public class Drowzee : PokemonMaster
{
    public string? Nickname {get;set;}

    private Drowzee() { } //For EF Core
    public Drowzee(string nickname, string ownerId) 
    : base("Drowzee", "Psychic", 60, 48, 45, 43, 90, 42, ownerId, 20, "Insomnia")
    {
        Nickname = nickname;
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}