using Server;
using PokemonPocket;
using FeatCalculator;

namespace Models;

public class AuroraBeam : Skill
{
    private AuroraBeam() { } // For EF Core
    public AuroraBeam(string PokemonId) : base("Aurora Beam", "Ice", 65, 1, 20, 1, 0, 0, "The user attacks with a beam of light that has a chance to lower the target's Attack.", PokemonId)    
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

        if (Random.Shared.Next(100) < 10 && target.AttackStage < 6)
        {
            target.AttackStage -= 1;
            target.Attack = (float)(target.MaxAttack * Calculator.CalculateStage(target.AttackStage));
            await UserSession.SendMessageAsync($"Your {user.Name} used Aurora Beam on {target.Name}, dealing {damage} damage and lowering its Attack to {target.Attack}.");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Aurora Beam on your {target.Name}, dealing {damage} damage and lowering its Attack to {target.Attack}.");
        }
        else
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Aurora Beam on {target.Name}, dealing {damage} damage.");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Aurora Beam on your {target.Name}, dealing {damage} damage.");
        }
    }
}