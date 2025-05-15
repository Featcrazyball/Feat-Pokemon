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
        while (true)
        {
        }
    }
}