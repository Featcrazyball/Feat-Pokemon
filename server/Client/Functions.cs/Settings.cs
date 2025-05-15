using Database;
using Models;
using System.Text;

namespace Server;

public class Settings
{
    public static async Task SettingsMenu(ClientSession session)
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
            StringBuilder sendMessage = new StringBuilder();
            sendMessage.Append(@$"
╔══════════════════════════════════════════════════════════════════════════════╗
║                                                                              ║
║      ✨✨✨✨✨✨✨✨✨    SETTINGS MENU    ✨✨✨✨✨✨✨✨✨               ║
║                                                                              ║
╠══════════════════════════════════════════════════════════════════════════════╣
║                                                                              ║
║    [1] 👤 USERNAME   │ Change Username                                       ║
║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║
║    [2] 📧 EMAIL      │ Change Email                                          ║
║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║
║    [3] 🔑 PASSWORD   │ Change Password                                       ║
║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║");

            if (user.God) 
            {
                sendMessage.Append("\n║    [4] 👑 GOD       │ Enter a New Realm                                      ║\n");
                sendMessage.Append("║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║\n");
            }

            sendMessage.Append($"║    [{(user.God ? "5" : "4")}] ↩️ BACK       │ Return to Trainer Menu                                ║");
            sendMessage.Append($"\n║                                                                              ║");
            sendMessage.Append($"\n╚══════════════════════════════════════════════════════════════════════════════╝");

            await session.SendMessageAsync(sendMessage.ToString());
            string choice = await session.GetInputAsync("Choice:");

            switch (choice)
            {
                case "1":
                    await ChangeUsername(session, user);
                    break;
                case "2":
                    await ChangeEmail.Email(session);
                    break;
                case "3":
                    await ChangePassword(session, user);
                    break;
                case "4":
                    if (user.God)
                        await EnterNewRealm(session, user);
                    else
                        continue;
                    break;
                case "5":
                    return;
                default:
                    continue;
            }
        }
    }
}