using Server;
using System.Threading.Tasks;

namespace Server;

public class Game
{
    public static async Task Rooms(ClientSession user)
    {
        await user.SendMessageAsync("┌───────────────────────────────────┐\n│         Rooms         │\n└───────────────────────────────────┘");
    }
}