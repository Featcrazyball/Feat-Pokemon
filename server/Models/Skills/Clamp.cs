using Server;
using PokemonPocket;

namespace Models;

public class Clamp : Skill
{
    private Clamp() { } // For EF Core
    public Clamp(string PokemonId) : base("Clamp", "Water", 35, 0.85, 15, 1, 0, 0, "The target is clamped and squeezed by the user's very thick and sturdy shell for four to five turns.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;

        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        if (target.BindActive) {await UserSession.SendMessageAsync($"Opponent already being bind, thus binding fails"); return;}
        
        // Accuracy check
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Clamp") == false)
            return;

        int turns;
        float chance = Random.Shared.Next(0, 100);
        if (chance > 87.5) turns = 5;
        else if (chance > 75) turns = 4;
        else if (chance > 37.5) turns = 3;
        else turns = 2;

        float damage = await SkillHelper.FeatCalculateSpecialDamage(
            BasePower, 
            user, 
            target, 
            await SkillHelper.GetEffectiveness(UserSession, TargetSession, "Water", target.Type?.Split('/') ?? Array.Empty<string>()),  
            UserSession, 
            TargetSession
        );

        if (target.Substitude == true)
        {
            if (target.SubstituteHealth <= damage) 
            {
                target.Substitude = false;
                target.SubstituteHealth = 0;

                target.BindDamage = damage;
                target.BindTurns = turns;
                target.BindActive = true;

                await UserSession.SendMessageAsync($"Your {user.Name} used Clamp and broke {target.Name}'s Substitude and binding {TargetSession.Username}'s {target.Name}!");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Clamp broke your {target.Name}'s Substitude and bind your {target.Name}!");
            }
            else
            {
                target.SubstituteHealth -= damage;

                await UserSession.SendMessageAsync($"Your {user.Name} used Clamp on {target.Name}'s Substitude, dealing {damage} damage.");
                await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Clamp on your {target.Name}'s Substitude, dealing {damage} damage.");
                if (target.SubstituteHealth < 0) target.SubstituteHealth = 0;
            }
        } 
        else
        {
            target.Health -= damage;
            await SkillHelper.ProcessRage(target, TargetSession, UserSession);
            target.BindDamage = damage;
            target.BindTurns = turns;
            target.BindActive = true;

            await UserSession.SendMessageAsync($"Your {user.Name} used Bind and is now squeezing the target!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Bind and is now squeezing the target!");
        }

    }
}