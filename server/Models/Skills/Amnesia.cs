using Server;
using PokemonPocket;

namespace Models;

public class Amnesia : Skill
{
    private Amnesia() { } // For EF Core
    public Amnesia(string PokemonId) : base("Amnesia", "Psychic", 0, -1, 20, 1, 0, 0, "The user temporarily forgets its worries and focuses on its inner self, raising its Special Defense.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;

        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        for (int i = 0; i < 2; i++)
        {
            if (user.SpecialDefenseStage >= 6) {break;}
            user.SpecialDefenseStage += 1;
            user.SpecialDefense = (float)(user.MaxSpecialDefense * SkillHelper.CalculateStage(user.SpecialDefenseStage));
        }

        await UserSession.SendMessageAsync($"Your {user.Name} used Amnesia, increasing its Special Defense to {user.SpecialDefense}.");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Amnesia, increasing its Special Defense to {user.SpecialDefense}.");
    }
}