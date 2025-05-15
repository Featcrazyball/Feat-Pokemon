using Server;
using Database;
using System.Text;
using Models;
using PokemonPocket;

namespace Server;

public class Lineup
{
    public static async Task LineupMenu(ClientSession session)
    {
        User user;
        var PokemonList = new List<PokemonMaster>();
        using (var context = new DatabaseContext())
        {
            user = context.Users.FirstOrDefault(u => u.Username == session.Username)!;
            PokemonList = context.PokemonMaster.Where(p => p.OwnerId == user.Id).ToList();

            if (user == null)
            {
                await session.SendMessageAsync("There has been an error locating your account. Please try again.");
                return;
            }
        }

        while (true)
        {
            var sendMessage = new StringBuilder();
            sendMessage.Append(@"
╔══════════════════════════════════════════════════════════════════════════════╗
║                                                                              ║
║      ✨✨✨✨✨✨✨✨✨    LINEUP MENU    ✨✨✨✨✨✨✨✨✨              ║
║                                                                              ║
╠══════════════════════════════════════════════════════════════════════════════╣
║                                                                              ║");

            string Selected = "";
            // Display the Pokémon lineup
            for (int i = 0; i < PokemonList.Count; i++)
            {
                var pokemon = PokemonList[i];
                if (pokemon.Selected)
                {
                    Selected = "(Selected)";
                }

                sendMessage.Append($"\n║    [{i + 1}, -3] {pokemon.Name, 30} │ Level: {pokemon.Level, -3} ║{Selected, -22}║");
                sendMessage.Append($"\n║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━        ║");
            }

            sendMessage.Append($"\n║    [0] BACK     │ Return to Trainer Menu                                  ║");
            sendMessage.Append($"\n║                                                                              ║");
            sendMessage.Append("╠══════════════════════════════════════════════════════════════════════════════╣\n");
            sendMessage.Append("║    Enter a POKÉMON's Number to add it to your lineup.                         ║");
            sendMessage.Append("║    If POKÉMON is already selected, selecting it again will unselect it.       ║");
            sendMessage.Append("║   -----------------------------------------------------------------------     ║");
            sendMessage.Append("║    To Make a POKÉMON your Starter POKÉMON, include 'Start' during input       ║");
            sendMessage.Append("\n╚══════════════════════════════════════════════════════════════════════════════╝\n");


            await session.SendMessageAsync(sendMessage.ToString());

            string choice = await session.GetInputAsync("Choice:");

            if (choice == "0")
            {
                break; // Exit the lineup menu
            }
        }
    }
}