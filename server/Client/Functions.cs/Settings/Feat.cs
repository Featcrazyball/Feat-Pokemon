using Database;
using Models;
using System.Text;

namespace Server;

public class God
{
    public static async Task EnterNewRealm(ClientSession session)
    {
        using var context = new DatabaseContext();
        User user = context.Users.FirstOrDefault(u => u.Username == session.Username)!;
        if (user == null)
        {
            await session.SendMessageAsync("There has been an error locating your account. Please try again.");
            return;
        }

        StringBuilder sendMessage = new StringBuilder();
        sendMessage.Append(@$"
╔══════════════════════════════════════════════════════════════════════════════╗
║                                                                              ║
║      ✨✨✨✨✨✨✨✨✨    GOD REALM    ✨✨✨✨✨✨✨✨✨               ║
║                                                                              ║
╠══════════════════════════════════════════════════════════════════════════════╣
║                                                                              ║
║    [1] 🔱 POKÉMON GOD  │ Give Life to Pokemon                                ║
║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║
║    [2] ⚖️ USER GOD     │ Decide the Life and Death of Mortals                ║
║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║
║    [3] ↩️ BACK         │ Return to Settings Menu                             ║
║                                                                              ║
╚══════════════════════════════════════════════════════════════════════════════╝");

        while (true)
        {
            await session.SendMessageAsync(sendMessage.ToString());
            string choice = await session.GetInputAsync("Choice:");

            switch (choice)
            {
                case "1":
                    break;
                case "2":
                    break;
                default:
                    await session.SendMessageAsync("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}