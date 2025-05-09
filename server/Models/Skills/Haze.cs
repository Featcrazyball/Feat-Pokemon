using Server;
using PokemonPocket;

namespace Models;

public class Haze : Skill
{
    private Haze() { } // For EF Core
    public Haze(string PokemonId) : base("Haze", "Ice", 0, -1, 30, 1, 0, 0, "The user creates a haze that eliminates every stat change among all the Pokémon engaged in battle.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Reset all stat stages of user
        user.AttackStage = 0;
        user.Attack = user.MaxAttack;
        
        user.DefenseStage = 0;
        user.Defense = user.MaxDefense;
        
        user.SpecialAttackStage = 0;
        user.SpecialAttack = user.MaxSpecialAttack;
        
        user.SpecialDefenseStage = 0;
        user.SpecialDefense = user.MaxSpecialDefense;
        
        user.SpeedStage = 0;
        user.Speed = user.MaxSpeed;
        
        user.AccuracyStage = 0;
        user.EvasionStage = 0;
        
        // Reset all stat stages of target
        target.AttackStage = 0;
        target.Attack = target.MaxAttack;
        
        target.DefenseStage = 0;
        target.Defense = target.MaxDefense;
        
        target.SpecialAttackStage = 0;
        target.SpecialAttack = target.MaxSpecialAttack;
        
        target.SpecialDefenseStage = 0;
        target.SpecialDefense = target.MaxSpecialDefense;
        
        target.SpeedStage = 0;
        target.Speed = target.MaxSpeed;
        
        target.AccuracyStage = 0;
        target.EvasionStage = 0;
        
        // Handle burn and paralysis effects which affect stats
        if (target.Burning && target.BurningAttack)
        {
            target.Attack *= 0.5f;
        }
        
        if (target.Paralyzed && target.ParalyzeSpeed)
        {
            target.Speed *= 0.25f;
        }
        
        if (user.Burning && user.BurningAttack)
        {
            user.Attack *= 0.5f;
        }
        
        if (user.Paralyzed && user.ParalyzeSpeed)
        {
            user.Speed *= 0.5f;
        }
        
        await UserSession.SendMessageAsync($"Your {user.Name} used Haze! All stat changes were eliminated!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Haze! All stat changes were eliminated!");
    }
}