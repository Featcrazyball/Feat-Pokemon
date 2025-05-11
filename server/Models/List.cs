namespace Models;

public static class ListofStuff
{
    // Complete list of all Gen 1 Pokemon skills
    public static readonly string[] AllSkills = new[]
    {
        "absorb", "acid", "acid armor", "agility", "amnesia", "aurora beam",
        "barrage", "barrier", "bide", "bind", "bite", "blizzard",
        "body slam", "bone club", "bonemerang", "bubble", "bubble beam", 
        "clamp", "comet punch", "confuse ray", "confusion", "constrict", "conversion", "counter",
        "crabhammer", "cut", "defense curl", "dig", "disable", "dizzy punch",
        "double-edge", "double kick", "double slap", "double team", "dragon rage", "dream eater",
        "drill peck", "earthquake", "egg bomb", "ember", "explosion", 
        "fire blast", "fire punch", "fire spin", "fissure", "flamethrower", "flash", "fly",
        "focus energy", "fury attack", "fury swipes", "glare", "growl", "growth",
        "guillotine", "gust", "harden", "haze", "headbutt", "high jump kick",
        "horn attack", "horn drill", "hydro pump", "hyper beam", "hyper fang", "hypnosis",
        "ice beam", "ice punch", "jump kick", "karate chop", "kinesis", 
        "leech life", "leech seed", "leer", "lick", "light screen", "lovely kiss", "low kick",
        "meditate", "mega drain", "mega kick", "mega punch", "metronome", "mimic",
        "minimize", "mirror move", "mist", "night shade", "pay day", "peck",
        "petal dance", "pin missile", "poison gas", "poison powder", "poison sting", "pound",
        "psybeam", "psychic", "psywave", "quick attack", "rage", "razor leaf",
        "razor wind", "recover", "reflect", "rest", "roar", "rock slide",
        "rock throw", "rolling kick", "sand attack", "scratch", "screech", "seismic toss",
        "self-destruct", "sharpen", "sing", "skull bash", "sky attack", "slam",
        "slash", "sleep powder", "sludge", "smog", "smokescreen", "soft-boiled",
        "solar beam", "sonic boom", "spike cannon", "splash", "spore", "stomp",
        "strength", "string shot", "struggle", "stun spore", "submission", "substitute",
        "super fang", "supersonic", "surf", "swift", "swords dance", "tackle",
        "tail whip", "take down", "teleport", "thrash", "thunder", "thunderbolt",
        "thunder punch", "thunder shock", "thunder wave", "toxic", "transform", "tri attack",
        "twineedle", "vine whip", "vise grip", "waterfall", "water gun", "whirlwind",
        "wing attack", "withdraw", "wrap"
    };

    public static string[] GetSkillsByType(string type)
    {
        return type.ToLower() switch
        {
            "normal" => new[] { "barrage", "bind", "body slam", "comet punch", "constrict", "cut", "double-edge", 
                                "double slap", "fury attack", "fury swipes", "guillotine", "headbutt", "horn attack", 
                                "horn drill", "hyper beam", "hyper fang", "mega kick", "mega punch", "metronome", 
                                "mimic", "pay day", "pound", "quick attack", "rage", "razor wind", "scratch", 
                                "self-destruct", "skull bash", "slam", "slash", "spike cannon", "stomp", "strength", 
                                "struggle", "tackle", "tail whip", "take down", "thrash", "transform", "tri attack", 
                                "vise grip", "wrap" },
            "fire" => new[] { "ember", "fire blast", "fire punch", "fire spin", "flamethrower" },
            "water" => new[] { "bubble", "bubble beam", "clamp", "crabhammer", "hydro pump", "surf", "water gun", "waterfall" },
            "electric" => new[] { "thunder", "thunderbolt", "thunder punch", "thunder shock", "thunder wave" },
            "grass" => new[] { "absorb", "leech seed", "mega drain", "petal dance", "razor leaf", "sleep powder", 
                               "solar beam", "spore", "stun spore", "vine whip" },
            "ice" => new[] { "aurora beam", "blizzard", "ice beam", "ice punch" },
            "fighting" => new[] { "counter", "double kick", "high jump kick", "jump kick", "karate chop", "low kick", 
                                 "rolling kick", "seismic toss", "submission" },
            "poison" => new[] { "acid", "acid armor", "poison gas", "poison powder", "poison sting", "sludge", "smog", "toxic" },
            "ground" => new[] { "bone club", "bonemerang", "dig", "earthquake", "fissure", "sand attack" },
            "flying" => new[] { "drill peck", "fly", "gust", "mirror move", "peck", "sky attack", "wing attack" },
            "psychic" => new[] { "agility", "amnesia", "barrier", "confusion", "dream eater", "hypnosis", "kinesis", 
                                "light screen", "meditate", "night shade", "psybeam", "psychic", "psywave", "reflect", "rest", "teleport" },
            "bug" => new[] { "leech life", "pin missile", "string shot", "twineedle" },
            "rock" => new[] { "rock slide", "rock throw" },
            "ghost" => new[] { "confuse ray", "lick" },
            "dragon" => new[] { "dragon rage" },
            "status" => new[] { "conversion", "defense curl", "disable", "flash", "focus energy", "growl", "growth", 
                               "harden", "haze", "leer", "lovely kiss", "minimize", "mist", "recover", "roar", "screech", 
                               "sharpen", "sing", "smokescreen", "soft-boiled", "splash", "substitute", "super fang", 
                               "supersonic", "swift", "swords dance", "withdraw" },
            _ => Array.Empty<string>()
        };
    }

    // Complete list of all Gen 1 Pokemon
    public static readonly string[] AllPokemon = new[]
    {
        "abra", "aerodactyl", "alakazam", "arbok", "arcanine", "articuno", 
        "beedrill", "bellsprout", "blastoise", "bulbasaur", "butterfree", 
        "caterpie", "chansey", "charizard", "charmander", "charmeleon", 
        "clefable", "clefairy", "cloyster", "cubone", 
        "dewgong", "diglett", "ditto", "dodrio", "Doduo", "dragonair", 
        "dragonite", "dratini", "drowzee", "dugtrio", 
        "eevee", "ekans", "electabuzz", "electrode", "exeggcute", "exeggutor", 
        "farfetch", "fearow", "flareon", 
        "gastly", "gengar", "geodude", "gloom", "golbat", "goldeen", 
        "golduck", "golem", "graveler", "grimer", "growlithe", "gyarados", 
        "haunter", "hitmonchan", "hitmonlee", "horsea", "hypno", 
        "ivysaur", "jigglypuff", "jolteon", "jynx", 
        "kabuto", "kabutops", "kadabra", "kakuna", "kangaskhan", "kingler", 
        "koffing", "krabby", 
        "lapras", "lickitung", 
        "machamp", "machoke", "machop", "magikarp", "magmar", "magnemite", 
        "magneton", "mankey", "marowak", "meowth", "metapod", "mew", "mewtwo", 
        "moltres", "mr mime", "muk", 
        "nidoking", "nidoqueen", "nidoranf", "nidoranm", "nidorina", "nidorino", 
        "ninetales", 
        "oddish", "omanyte", "omastar", "onix", 
        "paras", "parasect", "persian", "pidgeot", "pidgeotto", "pidgey", 
        "pikachu", "pinsir", "poliwag", "poliwhirl", "poliwrath", "ponyta", 
        "porygon", "primeape", "psyduck", 
        "raichu", "rapidash", "raticate", "rattata", "rhydon", "rhyhorn", 
        "sandshrew", "sandslash", "scyther", "seadra", "seaking", "seel", 
        "shellder", "slowbro", "slowpoke", "snorlax", "spearow", "squirtle", 
        "starmie", "staryu", 
        "tangela", "tauros", "tentacool", "tentacruel", 
        "vaporeon", "venomoth", "venonat", "venusaur", "victreebel", "vileplume", 
        "voltorb", "vulpix", 
        "wartortle", "weedle", "weepinbell", "weezing", "wigglytuff", 
        "zapdos", "zubat"
    };
}