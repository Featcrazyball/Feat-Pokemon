using Server;
using PokemonPocket;

namespace Models;

public class Transform : Skill
{
    private Transform() { } // For EF Core
    public Transform(string PokemonId) : base("Transform", "Normal", 0, 1, 10, 1, 0, 0, "The user transforms into a copy of the target right down to having the same move set.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        // Check if substitute is present
        if (target.Substitude)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Transform, but {target.Name}'s substitute blocked it!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Transform, but your {target.Name}'s substitute blocked it!");
            return;
        }

        // Apply transform effects
        user.Transform = true;

        // Copy stat stages
        user.AttackStage = target.AttackStage;
        user.DefenseStage = target.DefenseStage;
        user.SpecialAttackStage = target.SpecialAttackStage;
        user.SpecialDefenseStage = target.SpecialDefenseStage;
        user.SpeedStage = target.SpeedStage;
        user.AccuracyStage = target.AccuracyStage;
        user.EvasionStage = target.EvasionStage;
        
        // Copy stats (but keep own HP)
        user.MaxAttack = target.MaxAttack;
        user.MaxDefense = target.MaxDefense;
        user.MaxSpecialAttack = target.MaxSpecialDefense;
        user.MaxSpecialDefense = target.MaxSpecialAttack;
        user.MaxSpeed = target.MaxSpeed;
        
        user.Attack = target.Attack;
        user.Defense = target.Defense;
        user.SpecialAttack = target.SpecialAttack;
        user.SpecialDefense = target.SpecialDefense;
        user.Speed = target.Speed;

        user.Name = target.Name;
        user.Type = target.Type;
        user.CritDmg = target.CritDmg;
        user.CritRate = target.CritRate;
        
        // Copy moves but with 5 PP each
        foreach (var skill in target.Skills)
        {
            if (skill.PowerPoints > 0)
            {
                var tempskill = user.ArenaTempSkillGain(skill.Name!);
                if (tempskill == null)
                {
                    await UserSession.SendMessageAsync($"Your {user.Name} used Transform, but it failed to copy {target.Name}'s move {skill.Name}!");
                    await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Transform, but it failed to copy your {target.Name}'s move {skill.Name}!");
                    continue;
                }
                tempskill.Transform = true;
                tempskill.PowerPoints = 5;
            }
        }
        
        await UserSession.SendMessageAsync($"Your {user.Name} transformed into {target.Name}!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} transformed into your {target.Name}!");
    }
}