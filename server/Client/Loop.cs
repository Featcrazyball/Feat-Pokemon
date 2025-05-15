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
╚══════════════════════════════════════════════════════════════════════════════╝
");
            string choice = await session.GetInputAsync("Choice:");
            
            switch (choice.ToLower())
            {
                case "1":
                    // View owner's pokemon and evolve
                    // Damage calculations for Assignment
                    // Remember Feat's Version and Assignment Details
                    await ServerPokemon.PokemonMenu(session);
                    break;
                case "2":
                    await Shop.ShopMenu(session);
                    break;
                case "3":
                    await Chat.ChatMenu(session);
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
                    // On Feat's Version or not
                    // Change username, email, password
                    break;
                case "7" or "q":
                    await session.SendMessageAsync("Thank you for playing!");
                    exit = true;
                    break;
                default:
                    break;
            }
        }
    }
    
}