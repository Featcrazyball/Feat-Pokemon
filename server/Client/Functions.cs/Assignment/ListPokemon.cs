using Database;
using PokemonPocket;
using Models;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Server;

public class AssignmentList
{
    public static async Task ListPokemonsInPocket(ClientSession session)
    {
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

        await session.SendMessageAsync("");
        StringBuilder sb = new StringBuilder();

        // List all Pokemon in the user's pocket in descending order of their level
        foreach (var pokemon in userPokemons)
        {
            sb.AppendLine($"Name: {pokemon.Name}");
            sb.AppendLine($"HP: {pokemon.MaxHealth}");
            sb.AppendLine($"Experience: {pokemon.Experience}");
            sb.AppendLine($"Skill: {pokemon.Skill}");
            sb.AppendLine($"---------------------------------");
        }

        await session.SendMessageAsync(sb.ToString());
    }
}