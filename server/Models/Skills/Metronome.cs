using Server;
using PokemonPocket;

namespace Models;

public class Metronome : Skill
{
    private static readonly string[] AllMoves = new string[]
    {
        "Pound", "Karate Chop", "Double Slap", "Comet Punch", "Mega Punch",
        "Pay Day", "Fire Punch", "Ice Punch", "Thunder Punch", "Scratch", "Vice Grip",
        "Guillotine", "Razor Wind", "Swords Dance", "Cut", "Gust", "Wing Attack",
        "Whirlwind", "Fly", "Bind", "Slam", "Vine Whip",
        "Stomp", "Double Kick", "Mega Kick", "Jump Kick", "Rolling Kick",
        "Sand-Attack", "Headbutt", "Horn Attack", "Fury Attack", "Horn Drill",
        "Tackle", "Body Slam", "Wrap", "Take Down", "Thrash",
        "Double-Edge", "Tail Whip", "Poison Sting", "Twineedle", "Pin Missile",
        "Leer", "Bite", "Growl", "Roar", "Sing",
        "Supersonic", "Sonic Boom", "Disable", "Acid", "Ember",
        "Flamethrower", "Mist", "Water Gun", "Hydro Pump", "Surf",
        "Ice Beam", "Blizzard", "Psybeam", "Bubble Beam", "Aurora Beam",
        "Hyper Beam", "Peck", "Drill Peck", "Submission", "Low Kick",
        "Counter", "Seismic Toss", "Strength", "Absorb", "Mega Drain",
        "Leech Seed", "Growth", "Razor Leaf", "Solar Beam", "Poison Powder",
        "Stun Spore", "Sleep Powder", "Petal Dance", "String Shot", "Dragon Rage",
        "Fire Spin", "Thunder Shock", "Thunderbolt", "Thunder Wave", "Thunder",
        "Rock Throw", "Earthquake", "Fissure", "Dig", "Toxic",
        "Confusion", "Psychic", "Hypnosis", "Meditate", "Agility",
        "Rage", "Teleport", "Night Shade", "Mimic", "Screech", 
        "Double Team", "Recover", "Harden", "Minimize", "SmokeScreen",
        "Confuse Ray", "Withdraw", "Defense Curl", "Barrier", "Light Screen",
        "Haze", "Reflect", "Focus Energy", "Bide", "Mirror Move",
        "Self-Destruct", "Egg Bomb", "Lick", "Smog", "Sludge",
        "Bone Club", "Fire Blast", "Waterfall", "Clamp", "Swift",
        "Skull Bash", "Spike Cannon", "Constrict", "Amnesia", "Kinesis",
        "Soft-Boiled", "Hi Jump Kick", "Glare", "Dream Eater", "Poison Gas",
        "Barrage", "Leech Life", "Lovely Kiss", "Sky Attack", "Transform",
        "Bubble", "Dizzy Punch", "Spore", "Flash", "Psywave",
        "Splash", "Acid Armor", "Crabhammer", "Explosion", "Fury Swipes", 
        "Bonemerang", "Rest", "Rock Slide", "Hyper Fang", "Sharpen",
        "Conversion", "Tri Attack", "Super Fang", "Slash", "Substitute",
        "Quick Attack",
    };
    
    private Metronome() { } // For EF Core
    public Metronome(string PokemonId) : base("Metronome", "Normal", 0, 1, 10, 1, 0, 0, "The user waggles a finger and stimulates its brain into randomly using any move.", PokemonId)    
    {
        this.PokemonId = PokemonId;
    }

    public override async Task SkillEfect(PokemonMaster target, PokemonMaster user, ClientSession UserSession, ClientSession TargetSession)
    {
        PowerPoints -= 1;
        
        // Update last move and first move
        await SkillHelper.MoveUpdater(this, user, UserSession, TargetSession);

        await UserSession.SendMessageAsync($"Your {user.Name} used Metronome!");
        await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Metronome!");
        
        // Select a random move
        string randomMove = AllMoves[Random.Shared.Next(AllMoves.Length)];
        Skill? moveToUse = user.ArenaTempSkillGain(randomMove.ToLower());

        if (moveToUse == null)
        {
            await UserSession.SendMessageAsync($"Your {user.Name} used Metronome, but it couldn't use the move!");
            await TargetSession.SendMessageAsync($"{UserSession.Username}'s {user.Name} used Metronome, but it couldn't use the move!");
            return;
        }

        moveToUse.Metronome = true;
        // Execute the selected move
        await moveToUse.SkillEfect(target, user, UserSession, TargetSession);
    }
}