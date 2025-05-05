using Database;
using Models;
namespace Server;

public static class SkillMethods
{
    public static Skill CreateSkill(string skillName, string pokemonId)
    {
        return skillName switch
        {
            // "Ember" => new Ember(pokemonId),
            // "Fire Blast" => new FireBlast(pokemonId),
            // "Water Gun" => new WaterGun(pokemonId),
            // "Hydro Pump" => new HydroPump(pokemonId),
            // "Tackle" => new Tackle(pokemonId),
            // "Quick Attack" => new QuickAttack(pokemonId),
            // "Absorb" => new Absorb(pokemonId),
            // "Razor Leaf" => new RazorLeaf(pokemonId),
            // "Thunder Shock" => new ThunderShock(pokemonId),
            // Add more skills as you create them
            _ => throw new ArgumentException($"Unknown skill: {skillName}")
        };
    }
}