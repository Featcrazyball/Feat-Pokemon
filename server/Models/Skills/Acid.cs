using Server;
using PokemonPocket;
using FeatCalculator;

namespace Models;

public class Acid : Skill
{
    private Acid() { } // For EF Core
    public Acid(string PokemonId) : base("Acid", "Poison", 40, 1, 30, 1, 0, 0, "The user spews a vile liquid that may lower the target's Special Defense.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, float Modifier, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        float damage = ((user.Level * 2 / 5 + 2) * BasePower * user.Attack / target.Defense / 50 + 2) * Modifier;
        if (damage < 0) damage = 0;

        if (Random.Shared.NextDouble() < user.CritRate) 
        {
            damage *= user.CritDmg; 
            await UserSession.SendMessageAsync("CRITICAL HIT!"); 
            await TargetSession.SendMessageAsync("CRITICAL HIT!");
        }
        target.Health -= damage;

        if (Random.Shared.NextDouble() <= 0.10)
            for (int i = 0; i < 1; i++)
                if (target.SpecialDefenseStage >= 6) {break;}
                target.SpecialDefenseStage -= 1;
                target.SpecialDefense = target.MaxSpecialDefense * (float)Calculator.CalculateStage(target.SpecialDefenseStage); 

        await UserSession.SendMessageAsync($"Your {user.Name} used Acid on {target.Name}, dealing {damage} damage and lowering its Special Defense to {target.SpecialDefense}.");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Acid on your {target.Name}, dealing {damage} damage and lowering its Special Defense to {target.SpecialDefense}.");
    }
}