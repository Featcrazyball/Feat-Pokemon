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
            new PokemonMaster("Pikachu", 2, "Raichu"),
            new PokemonMaster("Eevee", 3, "Flareon"),
            new PokemonMaster("Charmander", 1, "Charmeleon")
        };

        User user;
        using (var context = new DatabaseContext())
        {
            user = context.Users.FirstOrDefault(u => u.Username == session.Username)!;
            if (user == null)
            {
                await session.SendMessageAsync("User not found.");
                return;
            }
        }

        foreach (var pokemon in pokemonMasters)
        {
            pokemon.setOwner(user.Id!);
        }
    }
}
