using Database;
using Server;
namespace PokemonPocket;

public class Nidorino : PokemonMaster
{
    public override string? Requirements { get; set; } = "1 Moon Stone";
    private Nidorino() { } //For EF Core
    public Nidorino(string nickname, string ownerId) 
    : base("Nidorino", "Poison", 61, 72, 57, 55, 55, 65, ownerId, 23, "Poison Point")
    {
        Nickname = nickname;
        SkillPool = "Leer, Tackle, Horn Attack, Poison Sting, Focus Energy, Fury Attack, Horn Drill, Double Kick, Toxic, Body Slam, Take Down, Double-Edge, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Nidorino(NidoranM nidoran)
    : base("Nidorino", "Poison", 61, 72, 57, 55, 55, 65, nidoran.OwnerId ?? "Unknown", 23, "Poison Point")
    {
        Id = nidoran.Id;
        Level = 1;
        Nickname = nidoran.Nickname;
        Experience = nidoran.Experience;
        HpIV = nidoran.HpIV;
        AttackIV = nidoran.AttackIV;
        SpecialAttackIV = nidoran.SpecialAttackIV;
        DefenseIV = nidoran.DefenseIV;
        SpecialDefenseIV = nidoran.SpecialDefenseIV;
        SpeedIV = nidoran.SpeedIV;
        StatPoints = Random.Shared.Next(1, 10);
        StatsEarned = 0;
        SkillPool = "Leer, Tackle, Horn Attack, Poison Sting, Focus Energy, Fury Attack, Horn Drill, Double Kick, Toxic, Body Slam, Take Down, Double-Edge, Mimic, Double Team, Reflect, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public override async Task Evolve(ClientSession session)
    {
        using (var context = new DatabaseContext())
        {
            var item = context.Items.FirstOrDefault(i => i.Name == "Moon Stone" && i.OwnerId == OwnerId);
            if (item != null) {
                context.Items.Remove(item);
            } else {
                await session.SendMessageAsync($"{Nickname} needs a Moon Stone to evolve!");
                return;
            }

            var nidoking = new Nidoking(this);
            nidoking.EvolveLevelUp(Level-1); // Level up to current level

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(nidoking);
            foreach (var skill in nidoking.Skills)
            {
                context.Skills.Add(skill);
            }

            // Remove previous and add new Pokemon
            context.SaveChanges();
        }
        await session.SendMessageAsync($"{Nickname} has evolved from a Nidorino to a Nidoking!");
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}