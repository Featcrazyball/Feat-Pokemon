using Database;
using PokemonPocket;
using Models;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Server;

public class Assignment
{
    public static async Task AssignmentMenu(ClientSession session)
    {
        while (true)
        {
            await session.SendMessageAsync(@"
*****************************
Welcome to Pokemon Pocket App
*****************************
(1). Add Pokemon to my Pocket
(2). List Pokemon(s) in my Pocket
(3). Check if I can evolve pokemon
(4). Evolve Pokemon
Please only enter [1,2,3,4] or Q to quit:");

            string choice = await session.GetInputAsync("");

            if (choice.ToLower() == "q")
            {
                break;
            }

            if (choice == "Q")
            {
                break;
            }

            if (int.TryParse(choice, out int option))
            {
                switch (option)
                {
                    case 1:
                        await AssignmentAdd.AddPokemonToPocket(session);
                        break;
                    case 2:
                        await AssignmentList.ListPokemonsInPocket(session);
                        break;
                    case 3:
                        await AssignmentCheck.CheckEvolvePokemon(session);
                        break;
                    case 4:
                        await AssignmentEvolve.EvolvePokemon(session);
                        break;
                    default:
                        await session.SendMessageAsync("Invalid option. Please try again.");
                        break;
                }
            }
            else
            {
                await session.SendMessageAsync("Invalid input. Please enter a number or Q to quit.");
            }

        }
    }
}