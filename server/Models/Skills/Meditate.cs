using Server;
using PokemonPocket;

namespace Models;

public class Meditate : Skill
{
    private Meditate() { } // For EF Core
    public Meditate(string PokemonId) : base("Meditate", "Psychic", 0, 1, 40, 1, 0, 0, "The user meditates to awaken the power within. It raises the user's Attack stat.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Check if Attack can be raised further
        if (user.AttackStage >= 6)
        {
            await UserSession.SendMessageAsync($"Your {user.Name}'s Attack won't go any higher!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name}'s Attack won't go any higher!");
            return;
        }
        
        // Increase Attack by one stage
        user.AttackStage += 1;
        user.Attack = user.MaxAttack * (float)SkillHelper.CalculateStage(user.AttackStage);

        if (user.Burning) {user.Attack *= (float)0.5;}
        
        await UserSession.SendMessageAsync($"Your {user.Name} used Meditate! Its Attack rose by 1 Stage!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Meditate! Its Attack rose by 1 Stage!");
    }
}