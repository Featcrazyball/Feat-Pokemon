using Database;
using Models;
using System.Text;

namespace Server;

public class ChangeUsername
{
    public static async Task Username(ClientSession session)
    {
        using var context = new DatabaseContext();
        User user = context.Users.FirstOrDefault(u => u.Username == session.Username)!;

        if (user == null)
        {
            await session.SendMessageAsync("There has been an error locating your account. Please try again.");
            return;
        }

        while (true)
        {
            string username = await session.GetInputAsync("Enter your new username (\"cancel\" to return to Settings):");

            if (username == "cancel")
            {
                await session.SendMessageAsync("Returning to Settings...");
                break;
            }

            if (string.IsNullOrEmpty(username))
            {
                await session.SendMessageAsync("Username cannot be empty. Please try again.");
                continue;
            }

            if (username.Length < 3 || username.Length > 20)
            {
                await session.SendMessageAsync("Username must be between 3 and 20 characters long. Please try again.");
                continue;
            }

            if (context.Users.Any(u => u.Username == username))
            {
                await session.SendMessageAsync("Username already in use. Please try again.");
                continue;
            }

            if (username == "Featcrazyball")
            {
                await session.SendMessageAsync("Username cannot be Featcrazyball. Please try again.");
                continue;
            }

            user.Username = username;
            context.Users.Update(user);
            context.SaveChanges();

            session.Username = username;
            await session.SendMessageAsync($"Your username has been changed to {username}.");

            await session.GetInputAsync("Input any key to continue...");
            break;
        }
    }
}