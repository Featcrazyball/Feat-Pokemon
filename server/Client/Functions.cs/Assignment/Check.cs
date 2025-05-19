using Database;
using PokemonPocket;
using Models;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Server;

public class AssignmentCheck
{
    public static async Task CheckEvolvePokemon(ClientSession session)
    {
        List<PokemonMaster> pokemonMasters = new List<PokemonMaster>(){
            // Assignment
            new PokemonMaster("Pikachu", 2, "Raichu"),
            new PokemonMaster("Eevee", 3, "Flareon"),
            new PokemonMaster("Charmander", 1, "Charmeleon"),

        };

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
                .OrderByDescending(p => p.Experience)
                .ToList();
        }

        StringBuilder sb = new StringBuilder();

        int i = 0;
        foreach (var pokemon in userPokemons)
        {
            if (pokemonMasters.Any(p => p.Name == pokemon.Name))
            {
                var master = pokemonMasters.First(p => p.Name == pokemon.Name);

                if (master.NoToEvolve != 0)
                {
                    continue;
                }

                i++;
            }
            else
            {
                continue;
            }
        }

        // List all Pokemon in the user's pocket in descending order of their level
        foreach (var pokemon in userPokemons)
        {
            if (pokemonMasters.Any(p => p.Name == pokemon.Name))
            {
                // If in PokeMaster
                var master = pokemonMasters.First(p => p.Name == pokemon.Name);

                int noOfPokemon = userPokemons
                    .Where(p => p.Name == master.Name)
                    .ToList()
                    .Count();

                if (noOfPokemon >= master.NoToEvolve)
                {
                    int countTo = noOfPokemon / master.NoToEvolve;
                    int countHave = countTo * master.NoToEvolve;

                    string Display = i == 1 ? $"{pokemon.Name} --> {master.EvolveTo}" : $"{countHave} {pokemon.Name} --> {countTo} {master.EvolveTo}";
                    sb.AppendLine($"{Display}");
                }
            }
            else
            {
                int noOfPokemon = userPokemons
                    .Where(p => p.Name != pokemon.Name)
                    .ToList()
                    .Count();

                if (pokemon.Requirements == "Unevolvable")
                {
                    continue;
                }

                if (noOfPokemon >= 2)
                    {
                        int countTo = noOfPokemon / 2;
                        int countHave = countTo * 2;

                        string Display = i == 1 ? $"{pokemon.Name} --> {pokemon.EvolvesTo}" : $"{countHave} {pokemon.Name} --> {countTo} {pokemon.EvolvesTo}";
                        sb.AppendLine($"{Display}");
                    }
                
            }
        }

        if (sb.Length == 0)
        {
            await session.SendMessageAsync("No Pokemon can evolve.");
        }
        else
        {
            await session.SendMessageAsync(sb.ToString());
        }
    }
}