using Database;
using Server;
namespace PokemonPocket;

public class NidoranM : PokemonMaster
{
    public override string? Requirements { get; set; } = "Level 16";
    private NidoranM() { } //For EF Core
    public NidoranM(string nickname, string ownerId) 
    : base("NidoranM", "Poison", 46, 57, 40, 40, 40, 50, ownerId, 10, "Poison Point")
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

    public override async Task Evolve(ClientSession session)
    {
        if (Level >= 16) {
            using (var context = new DatabaseContext())
            {
                var nidorino = new Nidorino(this);
                nidorino.EvolveLevelUp(Level-1);

                context.PokemonMaster.Add(nidorino);
                foreach (var skill in nidorino.Skills)
                {
                    context.Skills.Add(skill);
                }

                // Remove previous and add new Pokemon
                context.PokemonMaster.Remove(this);
                context.SaveChanges();
            }
            await session.SendMessageAsync($"{Nickname} has evolved from a NidoranM to a Nidorino!");
        } else {
            await session.SendMessageAsync($"{Nickname} is not ready to evolve yet.");
        }
    }

    public override float calculateDamage(float SkillDamage) {
        return SkillDamage;
    }
}