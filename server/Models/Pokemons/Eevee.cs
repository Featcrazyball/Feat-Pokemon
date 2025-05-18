using Database;
using Server;
namespace PokemonPocket;

public class Eevee : PokemonMaster
{
    public override float HealthOverride {get;set;} = 55;
    public override string? Requirements { get; set; } = "1 Water Stone/Thunder Stone/Fire Stone";
    public override string? EvolvesTo {get;set;} = "Vaporeon/Jolteon/Flareon";
    private Eevee() { } //For EF Core
    public Eevee(string nickname, string ownerId) 
    : base("Eevee", "Normal", 55, 55, 50, 45, 65, 55, ownerId, 25, "Run Away")
    {
        Nickname = nickname;
        SkillPool = "Tackle, Sand Attack, Growl, Quick Attack, Bite, Tail Whip, Take Down, Double-Edge, Reflect, Mimic, Double Team, Bide, Rest, Substitute";

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }

    public Eevee(float HP, string nickname, string ownerId, int exp)
    : base("Eevee", "Normal", HP, 55, 50, 45, 65, 55, ownerId, 25, "Run Away")
    {
        Nickname = nickname;
        Experience = exp;
        SkillPool = "Tackle, Sand Attack, Growl, Quick Attack, Bite, Tail Whip, Take Down, Double-Edge, Reflect, Mimic, Double Team, Bide, Rest, Substitute";

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
        string choice = await session.GetInputAsync($"Eevee has 3 Evolution options: \n[1] Vaporeon (Water Stone)\n[2] Jolteon (Thunder Stone)\n[3] Flareon (Fire Stone). \nPlease choose one to evolve into.");

        switch (choice)
        {
            case "1" or "Vaporeon":
                using (var context = new DatabaseContext())
                {
                    var item = context.Items.FirstOrDefault(i => i.Name == "Water Stone" && i.OwnerId == OwnerId);
                    if (item != null) {
                        context.Items.Remove(item);
                    } else {
                        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} needs a Water Stone to evolve!");
                        return;
                    }

                    foreach (var skill in this.Skills)
                    {
                        context.Skills.Remove(skill);
                    }

                    var vaporeon = new Vaporeon(this);
                    vaporeon.MaxHealth = vaporeon.HealthOverride;
                    vaporeon.EvolveLevelUp(Level-1); // Level up to current level
                    context.PokemonMaster.Remove(this);
                    context.PokemonMaster.Add(vaporeon);
                    foreach (var skill in vaporeon.Skills)
                    {
                        context.Skills.Add(skill);
                    }

                    context.SaveChanges();
                }
                await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from an Eevee to a Vaporeon!");
                break;
            case "2" or "Jolteon":
                using (var context = new DatabaseContext())
                {
                    var item = context.Items.FirstOrDefault(i => i.Name == "Thunder Stone" && i.OwnerId == OwnerId);
                    if (item != null) {
                        context.Items.Remove(item);
                    } else {
                        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} needs a Thunder Stone to evolve!");
                        return;
                    }

                    foreach (var skill in this.Skills)
                    {
                        context.Skills.Remove(skill);
                    }

                    var jolteon = new Jolteon(this);
                    jolteon.MaxHealth = jolteon.HealthOverride;
                    jolteon.EvolveLevelUp(Level-1); // Level up to current level

                    context.PokemonMaster.Remove(this);
                    context.PokemonMaster.Add(jolteon);
                    foreach (var skill in jolteon.Skills)
                    {
                        context.Skills.Add(skill);
                    }

                    context.SaveChanges();
                }
                await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from an Eevee to a Jolteon!");
                break;
            case "3" or "Flareon":
                using (var context = new DatabaseContext())
                {
                    var item = context.Items.FirstOrDefault(i => i.Name == "Fire Stone" && i.OwnerId == OwnerId);
                    if (item != null) {
                        context.Items.Remove(item);
                    } else {
                        await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} needs a Fire Stone to evolve!");
                        return;
                    }

                    foreach (var skill in this.Skills)
                    {
                        context.Skills.Remove(skill);
                    }

                    var flareon = new Flareon(this);
                    flareon.MaxHealth = flareon.HealthOverride;
                    flareon.EvolveLevelUp(Level-1); // Level up to current level

                    context.PokemonMaster.Remove(this);
                    context.PokemonMaster.Add(flareon);
                    foreach (var skill in flareon.Skills)
                    {
                        context.Skills.Add(skill);
                    }

                    context.SaveChanges();
                }
                await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from an Eevee to a Flareon!");
                break;
            default:
                await session.SendMessageAsync("Invalid choice. Eevee remains unevolved.");
                break;
        }
    }

    public override async Task GodEvolve(ClientSession session)
    {
        while (true)
        {
            string choice = await session.GetInputAsync($"Eevee has 3 Evolution options: \n[1] Vaporeon (Water Stone)\n[2] Jolteon (Thunder Stone)\n[3] Flareon (Fire Stone). \nPlease choose one to evolve into.");

            switch (choice)
            {
                case "1" or "Vaporeon":
                    using (var context = new DatabaseContext())
                    {
                        foreach (var skill in this.Skills)
                        {
                            context.Skills.Remove(skill);
                        }

                        var vaporeon = new Vaporeon(this);
                        vaporeon.MaxHealth = vaporeon.HealthOverride;
                        vaporeon.EvolveLevelUp(Level - 1); // Level up to current level
                        context.PokemonMaster.Remove(this);
                        context.PokemonMaster.Add(vaporeon);
                        foreach (var skill in vaporeon.Skills)
                        {
                            context.Skills.Add(skill);
                        }

                        context.SaveChanges();
                    }
                    await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from an Eevee to a Vaporeon!");
                    return;
                case "2" or "Jolteon":
                    using (var context = new DatabaseContext())
                    {
                        foreach (var skill in this.Skills)
                        {
                            context.Skills.Remove(skill);
                        }

                        var jolteon = new Jolteon(this);
                        jolteon.MaxHealth = jolteon.HealthOverride;
                        jolteon.EvolveLevelUp(Level - 1); // Level up to current level

                        context.PokemonMaster.Remove(this);
                        context.PokemonMaster.Add(jolteon);
                        foreach (var skill in jolteon.Skills)
                        {
                            context.Skills.Add(skill);
                        }

                        context.SaveChanges();
                    }
                    await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from an Eevee to a Jolteon!");
                    return;
                case "3" or "Flareon":
                    using (var context = new DatabaseContext())
                    {
                        foreach (var skill in this.Skills)
                        {
                            context.Skills.Remove(skill);
                        }

                        var flareon = new Flareon(this);
                        flareon.MaxHealth = flareon.HealthOverride;
                        flareon.EvolveLevelUp(Level - 1); // Level up to current level

                        context.PokemonMaster.Remove(this);
                        context.PokemonMaster.Add(flareon);
                        foreach (var skill in flareon.Skills)
                        {
                            context.Skills.Add(skill);
                        }

                        context.SaveChanges();
                    }
                    await session.SendMessageAsync($"{(Nickname == "None" ? Name : Nickname)} has evolved from an Eevee to a Flareon!");
                    return;
                default:
                    await session.SendMessageAsync("Invalid choice. Please try again.");
                    continue;
            }
        }
        
    }

    public override float calculateDamage(float SkillDamage)
    {
        return 2 * SkillDamage;
    }
}
