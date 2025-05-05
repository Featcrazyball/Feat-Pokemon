using System.Net.Sockets;
using System.Text;
using System.Collections.Concurrent;

namespace Server;

public class Client
{

    public static async Task GameLoop(ClientSession session, string username)
    {
        bool exit = false;
        while (!exit)
        {
            var choice = await session.GetChoiceAsync(
                "Main Menu",
                "View Pokemon",
                "Catch Pokemon",
                "Battle",
                "Exit"
            );
            
            switch (choice)
            {
                case "1":
                    break;
                case "2":
                    break;
                case "3":
                    break;
                case "4":
                    break;
                case "Exit":
                    await session.SendMessageAsync("Thank you for playing!");
                    exit = true;
                    break;
                default:
                    await session.SendMessageAsync("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}