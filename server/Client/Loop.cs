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
            await session.SendMessageAsync(@"
╔══════════════════════════════════════════════════════════════════════════════╗
║                                                                              ║
║      ✨✨✨✨✨✨✨✨✨    TRAINER MENU    ✨✨✨✨✨✨✨✨✨                ║
║                                                                              ║
╠══════════════════════════════════════════════════════════════════════════════╣
║                                                                              ║
║    [1] 🔥 POKÉMON      │ View & Evolve Your Pokémon                          ║
║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║
║    [2] 💰 SHOP         │ Buy Items & Special Pokémon                         ║
║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║
║    [3] 💬 CHAT         │ Connect With Other Trainers                         ║
║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║
║    [4] 📋 LINEUP       │ Prepare Your Battle Team                            ║
║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║
║    [5] ⚔️ ARENA        │ Challenge Trainers Worldwide                        ║
║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║
║    [6] ⚙️ SETTINGS     │ Configure Your Trainer Profile                      ║
║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║
║    [7] 🚪 EXIT         │ Save & Return to Real World                         ║
║                                                                              ║
╠══════════════════════════════════════════════════════════════════════════════╣
║    🪙 COINS: " + FormatCoins(user.Coins) + @"                                                       ║
╚══════════════════════════════════════════════════════════════════════════════╝
");
            string choice = await session.GetInputAsync("Choice:");
            
            switch (choice)
            {
                case "1":
                    // View owner's pokemon and evolve
                    // Damage calculations for Assignment
                    // Remember Feat's Version and Assignment Details
                    break;
                case "2":
                    await Shop.ShopMenu(session);
                    // Shop (using coins/Stats) (god gets stuff for free)
                    // Coins can be used to buy items for xp which can also be used to buy items
                    // Coins can be earned via p2w or by winning battles
                    break;
                case "3":
                    // Chat Area
                    break;
                case "4":
                    // Selected Pokemon for battle 
                    break;
                case "5":
                    // Battle Area (3 Pokemon per fight) + 1 Starter Pokemon
                    // Battle with other players
                    // require manual refresh. has one creator and one joiner. 
                    // joiner will create the arena object
                    // Cannot enter if Feat's Version is true
                    break;
                case "6":
                    // Pikachu, Charmander and Eevee. Free for only gods
                    break;
                case "7":
                    // On Feat's Version or not
                    // Change username, email, password
                    break;
                case "8":
                    await session.SendMessageAsync("Thank you for playing!");
                    exit = true;
                    break;
                default:
                    break;
            }
        }
    }
    
    // Helper method to format coins display with proper width
    private static string FormatCoins(int coins)
    {
        int totalWidth = 12; // Adjust this value based on the desired total width
        string coinString = coins.ToString();
        int padding = totalWidth - coinString.Length;
        string formattedCoins = coinString.PadRight(padding);
        return formattedCoins;
    }
}