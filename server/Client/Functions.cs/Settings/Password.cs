using Database;
using Models;
using System.Text;

namespace Server;

public class ChangePassword
{
    public static async Task Password(ClientSession session)
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

            while (true)
            {
                string oldPassword = await session.GetInputAsync("Enter your old password (\"cancel\" to return to Settings):");

                if (oldPassword == "cancel")
                {
                    return;
                }

                if (oldPassword != user.Password!)
                {
                    await session.SendMessageAsync("Incorrect password. Please try again.");
                    continue;
                }

                if (string.IsNullOrEmpty(oldPassword))
                {
                    await session.SendMessageAsync("Old password cannot be empty. Please try again.");
                    continue;
                }

                string newPassword = await session.GetInputAsync("Enter your new password:");

                if (newPassword == "cancel")
                    return;
                string confirmPassword = await session.GetInputAsync("Confirm your new password:");

                if (confirmPassword == "cancel")
                    return;


                if (string.IsNullOrEmpty(newPassword))
                {
                    await session.SendMessageAsync("New password cannot be empty. Please try again.");
                    continue;
                }

                if (newPassword.Length < 8)
                {
                    await session.SendMessageAsync("New password must be at least 8 characters long. Please try again.");
                    continue;
                }

                if (newPassword != confirmPassword)
                {
                    await session.SendMessageAsync("Passwords do not match. Please try again.");
                    continue;
                }

                user.Password = newPassword;

                context.Users.Update(user);
                context.SaveChanges();
                await session.SendMessageAsync("Password updated successfully.");
                await session.GetInputAsync("Input any key to continue...");
                return;
            }
        }


    }
}