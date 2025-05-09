using Server;
using PokemonPocket;

namespace Models;

public class Growl : Skill
{
    private Growl() { } // For EF Core
    public Growl(string PokemonId) : base("Growl", "Normal", 0, 1, 40, 1, 0, 0, "The user growls in an endearing way, making the opposing team less wary. The opposing team's Attack is lowered.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Check accuracy
        if (await SkillHelper.CheckAccuracy(Accuracy, user, target, UserSession, TargetSession, skillName : "Growl") == false) return;

        bool Mist = false;
        for (int i = 0; i < 1; i++)
        {
            if (target.AttackStage <= -6) {break;}
            if (target.Mist) {Mist = true; break;}
            target.AttackStage -= 1;
            target.Attack = (float)(target.MaxAttack * SkillHelper.CalculateStage(target.AttackStage));
        }
        if (user.Burning) {user.Attack *= (float)0.5;}

        if (Mist)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Growl on {target.Name}, but its Attack was not lowered due to Mist!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Growl on your {target.Name}, but its Attack was not lowered due to Mist!");
        }
        else
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Growl, lowering {target.Name}'s Attack by 1 Stage.");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Growl, lowering your Attack by 1 Stage.");
        }
    }
}