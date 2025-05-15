using Database;
using Models;
using System.Text;

namespace Server;

public class ChangeEmail
{
    public static async Task Email(ClientSession session)
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
            string email = await session.GetInputAsync("Enter your new email address (\"cancel\" to return to Settings):");

            if (email == "cancel")
            {
                await session.SendMessageAsync("Returning to Settings...");
                break;
            }

            if (string.IsNullOrEmpty(email))
            {
                await session.SendMessageAsync("Email cannot be empty. Please try again.");
                continue;
            }

            if (!email.Contains("@") || !email.Contains(".") || email.IndexOf("@") > email.LastIndexOf(".") || email.Length < 6)
            {
                await session.SendMessageAsync("Invalid email format. Please try again.");
                continue;
            }

            if (context.Users.Any(u => u.Email == email))
            {
                await session.SendMessageAsync("Email already in use. Please try again.");
                continue;
            }

            user.Email = email;
            context.Users.Update(user);
            context.SaveChanges();
            await session.SendMessageAsync("Email updated successfully.");
            await session.GetInputAsync("Input any key to continue...");
            break;
        }
    }
}