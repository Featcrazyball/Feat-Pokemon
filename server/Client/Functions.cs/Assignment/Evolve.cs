using Database;
using PokemonPocket;
using Models;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Server;

public class AssignmentEvolve
{
    public static async Task EvolvePokemon(ClientSession session)
    {
        List<PokemonMaster> pokemonMasters = new List<PokemonMaster>(){
            // Assignment
            new PokemonMaster("Pikachu", 2, "Raichu"),
            new PokemonMaster("Eevee", 3, "Flareon"),
            new PokemonMaster("Charmander", 1, "Charmeleon"),
    
            // Self-Added
            new PokemonMaster("Abra", 2, "Kadabra"),
            new PokemonMaster("Bellsprout", 2, "Weepinbell"),
            new PokemonMaster("Bulbasaur", 2, "Ivysaur"),
            new PokemonMaster("Caterpie", 2, "Metapod"),
            new PokemonMaster("Charmeleon", 3, "Charizard"),
            new PokemonMaster("Clefairy", 2, "Clefable"),
            new PokemonMaster("Cubone", 2, "Marowak"),
            new PokemonMaster("Diglett", 2, "Dugtrio"),
            new PokemonMaster("Doduo", 2, "Dodrio"),
            new PokemonMaster("Dragonair", 3, "Dragonite"),
            new PokemonMaster("Dratini", 2, "Dragonair"),
            new PokemonMaster("Drowzee", 2, "Hypno"),
            new PokemonMaster("Ekans", 2, "Arbok"),
            new PokemonMaster("Exeggcute", 2, "Exeggutor"),
            new PokemonMaster("Ghastly", 2, "Haunter"),
            new PokemonMaster("Geodude", 2, "Graveler"),
            new PokemonMaster("Gloom", 2, "Vileplume"),
            new PokemonMaster("Goldeen", 3, "Seaking"),
            new PokemonMaster("Graveler", 3, "Golem"),
            new PokemonMaster("Grimer", 2, "Muk"),
            new PokemonMaster("Growlithe", 2, "Arcanine"),
            new PokemonMaster("Haunter", 3, "Gengar"),
            new PokemonMaster("Horsea", 2, "Seadra"),
            new PokemonMaster("Ivysaur", 3, "Venusaur"),
            new PokemonMaster("Jigglypuff", 2, "Wigglytuff"),
            new PokemonMaster("Kabuto", 2, "Kabutops"),
            new PokemonMaster("Kadabra", 3, "Alakazam"),
            new PokemonMaster("Kakuna", 2, "Beedrill"),
            new PokemonMaster("Koffing", 3, "Weezing"),
            new PokemonMaster("Krabby", 2, "Kingler"),
            new PokemonMaster("Machoke", 3, "Machamp"),
            new PokemonMaster("Machop", 2, "Machoke"),
            new PokemonMaster("Magikarp", 2, "Gyarados"),
            new PokemonMaster("Mankey", 2, "Primeape"),
            new PokemonMaster("Meowth", 2, "Persian"),
            new PokemonMaster("Metapod", 3, "Butterfree"),
            new PokemonMaster("NidoranF", 2, "Nidorina"),
            new PokemonMaster("Nidorina", 3, "Nidoqueen"),
            new PokemonMaster("Nidorino", 3, "Nidoking"),
            new PokemonMaster("NidoranM", 3, "Nidorino"),
            new PokemonMaster("Oddish", 2, "Gloom"),
            new PokemonMaster("Omanyte", 2, "Omastar"),
            new PokemonMaster("Paras", 2, "Parasect"),
            new PokemonMaster("Pidgeotto", 2, "Pidgeot"),
            new PokemonMaster("Pidgey", 2, "Pidgeotto"),
            new PokemonMaster("Poliwag", 2, "Poliwhirl"),
            new PokemonMaster("Poliwhirl", 3, "Poliwrath"),
            new PokemonMaster("Ponyta", 2, "Rapidash"),
            new PokemonMaster("Psyduck", 2, "Golduck"),
            new PokemonMaster("Rattata", 3, "Raticate"),
            new PokemonMaster("Rhyhorn", 2, "Rhydon"),
            new PokemonMaster("Sandshrew", 2, "Sandslash"),
            new PokemonMaster("Seel", 2, "Dewgong"),
            new PokemonMaster("Shellder", 2, "Cloyster"),
            new PokemonMaster("Slowpoke", 2, "Slowbro"),
            new PokemonMaster("Spearow", 2, "Fearow"),
            new PokemonMaster("Squirtle", 3, "Wartortle"),
            new PokemonMaster("Staryu", 2, "Starmie"),
            new PokemonMaster("Tentacool", 2, "Tentacruel"),
            new PokemonMaster("Venonat", 2, "Venomoth"),
            new PokemonMaster("Voltorb", 2, "Electrode"),
            new PokemonMaster("Vulpix", 2, "Ninetales"),
            new PokemonMaster("Wartortle", 3, "Blastoise"),
            new PokemonMaster("Weedle", 2, "Kakuna"),
            new PokemonMaster("Weepinbell", 3, "Victreebel"),
            new PokemonMaster("Zubat", 2, "Golbat"),
        };

        // Retrieve user and their pokemon from the database
        User user;
        List<PokemonMaster> userPokemons;
        using (var context = new DatabaseContext())
        {
            user = context.Users.FirstOrDefault(u => u.Username == session.Username)!;
            if (user == null)
            {
                await session.SendMessageAsync("User not found.");
                return;
            }

            userPokemons = context.PokemonMaster
                .Where(p => p.OwnerId == user.Id)
                .ToList();
        }

        // Check for which pokemon can evolve
        var evolutions = new Dictionary<string, int>();
        foreach (var pokemon in userPokemons)
        {
            var master = pokemonMasters.FirstOrDefault(p => p.Name == pokemon.Name);

            if (master == null)
            {
                continue;
            }

            if (evolutions.ContainsKey(master.Name!))
            {
                evolutions[master.Name!] += 1;
            }
            else
            {
                evolutions.Add(master.Name!, 1);
            }
        }

        // Evolve the pokemon
        foreach (var evolution in evolutions)
        {
            var master = pokemonMasters.FirstOrDefault(p => p.Name == evolution.Key);

            if (master == null || evolutions.Count == 0)
            {
                continue;
            }

            int totalPokemon = evolution.Value;
            int evolveCount = totalPokemon / master.NoToEvolve;

            if (evolveCount > 0)
            {
                int usedPokemonCount = evolveCount * master.NoToEvolve;
                try
                {
                    PokemonMaster.AssignmentEvolve(usedPokemonCount, evolveCount, master.Name!, master.EvolveTo!, user);
                } catch (Exception ex)
                {
                    await session.SendMessageAsync($"Error evolving Pokemon: {ex.Message}");
                    return;
                }
            }
        }


    }
}
