using Database;
using Models;

namespace Server;

public class Client
{

    public static async Task GameLoop(ClientSession session, string username)
    {
        bool exit = false;

        using var context = new DatabaseContext();
        var user = context.Users.FirstOrDefault(u => u.Username == username);

        if (user == null)
        {
            await session.SendMessageAsync("There has been an error locating your account. Please try again.");
            await session.SendMessageAsync("2q30-8b6r7-vyq20974ryf-b09qw8r7bq9-28-3v");
            return;
        }

        while (!exit)
        {
            var choice = await session.GetChoiceAsync(
                "Main Menu",
                "View Pokemon",
                "Catch Pokemon",
                "Battle",
                "Exit"
            );
            await session.SendMessageAsync("Please enter your choice:");
            
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