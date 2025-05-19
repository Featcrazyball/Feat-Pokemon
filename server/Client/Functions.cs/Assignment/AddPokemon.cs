using Database;
using PokemonPocket;
using Models;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Server;

public class AssignmentAdd
{
    public static async Task AddPokemonToPocket(ClientSession session)
    {
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

        string name = await session.GetInputAsync("Enter Pokemon's Name:");
        string HP = await session.GetInputAsync("Enter Pokemon's HP:");
        string Exp = await session.GetInputAsync("Enter Pokemon's Exp:");

        if (float.TryParse(HP, out float hp) && int.TryParse(Exp, out int exp))
        {
            try
            {
                user.AdminGetPokemon(name, user.Id!, hp, exp);
            }
            catch (Exception ex)
            {
                await session.SendMessageAsync($"Error adding Pokemon: {ex.Message}");
                return;
            }
        }
        else
        {
            await session.SendMessageAsync("Invalid input. Please enter valid numbers for HP and Exp.");
        }


    }
}