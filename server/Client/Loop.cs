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

        user!.InRoom = false;

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
║    [7] 📖 Assignment   │ Assignment Is Here                                  ║
║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║
║    [8] 🚪 EXIT         │ Save & Return to Real World                         ║
║                                                                              ║
╚══════════════════════════════════════════════════════════════════════════════╝
");

                string choice = await session.GetInputAsync("Choice:");

                switch (choice.ToLower())
                {
                    case "1":
                        await ServerPokemon.PokemonMenu(session);
                        break;
                    case "2":
                        await Shop.ShopMenu(session);
                        break;
                    case "3":
                        await Chat.ChatMenu(session);
                        break;
                    case "4":
                        await Lineup.LineupMenu(session);
                        break;
                    case "5":
                        // Battle Area (5 Pokemon per fight) + 1 Starter Pokemon
                        // Battle with other players
                        // require manual refresh. has one creator and one joiner. 
                        // joiner will create the arena object
                        // Cannot enter if Feat's Version is true

                        using (var db = new DatabaseContext())
                        {
                            // Check for selected Pokémon
                            var selectedPokemons = db.PokemonMaster
                                .Where(p => p.OwnerId == user.Id && p.Selected)
                                .ToList();

                            if (selectedPokemons.Count != 6)
                            {
                                await session.SendMessageAsync($"You need exactly 6 Pokémon in your lineup. You currently have {selectedPokemons.Count}.");
                                await session.SendMessageAsync("Please go to the Lineup menu to select your Pokémon.");
                                await session.GetInputAsync("\nInput any key to continue...");
                                break;
                            }

                            // Check for starter Pokémon
                            var starterPokemon = db.PokemonMaster
                                .Where(p => p.OwnerId == user.Id && p.Starter)
                                .ToList();

                            if (starterPokemon.Count != 1)
                            {
                                await session.SendMessageAsync($"You need exactly 1 starter Pokémon. You currently have {starterPokemon.Count}.");
                                await session.SendMessageAsync("Please go to the Lineup menu to set one Pokémon as your starter.");
                                await session.GetInputAsync("Input any key to continue...");
                                break;
                            }

                            await Game.Rooms(session);
                        }
                        break;

                    case "6":
                        await Settings.SettingsMenu(session);
                        break;

                    case "7":
                        await Assignment.AssignmentMenu(session);
                        break;
                    case "8" or "q":
                        await session.SendMessageAsync("Thank you for playing!");
                        exit = true;
                        break;
                    default:
                        break;
                }
            }
    }
    
}