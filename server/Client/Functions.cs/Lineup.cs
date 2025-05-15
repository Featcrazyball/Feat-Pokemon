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
║      ✨✨✨✨✨✨✨✨✨    LINEUP MENU    ✨✨✨✨✨✨✨✨✨                 ║
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
                    Selected = " (Selected)";
                    if (pokemon.Starter)
                    {
                        Selected = " (Starter)";
                    }
                } 
                else
                {
                    Selected = "";
                }

                sendMessage.Append($"\n║    {$"[{i+1}]",-3} {pokemon.Name, -30} ║ Level: {pokemon.Level, -3} ║{Selected, -21}    ║");
                sendMessage.Append($"\n║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━          ║");
            }

            sendMessage.Append($"\n║    [B] BACK     │ Return to Trainer Menu                                     ║");
            sendMessage.Append($"\n║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━          ║");

            sendMessage.Append("\n╠══════════════════════════════════════════════════════════════════════════════╣\n");
            sendMessage.Append("║                                                                              ║\n");
            sendMessage.Append("║    📋  LINEUP INSTRUCTIONS:                                                  ║\n");
            sendMessage.Append("║                                                                              ║\n");
            sendMessage.Append("║    🔢  Enter a Pokémon's number to add it to your lineup                     ║\n");
            sendMessage.Append("║    🔄  Selecting an already selected Pokémon will unselect it                ║\n");
            sendMessage.Append("║                                                                              ║\n");
            sendMessage.Append("║    ⭐  STARTER POKÉMON  ⭐                                                   ║\n");
            sendMessage.Append("║    Type the number followed by 'Start' to make a Pokémon your starter        ║\n");
            sendMessage.Append("║    Example: \"3 Start\" will make the 3rd Pokémon your starter                 ║\n");
            sendMessage.Append("║                                                                              ║\n");
            sendMessage.Append("╚══════════════════════════════════════════════════════════════════════════════╝\n");


            await session.SendMessageAsync(sendMessage.ToString());

            string choice = await session.GetInputAsync("Choice:");

            if (choice.ToLower() == "b")
            {
                break;
            }

            // Check if the choice is a number
            if (int.TryParse(choice, out int selectedIndex) && selectedIndex > 0 && selectedIndex <= PokemonList.Count)
            {
                var selectedPokemon = PokemonList[selectedIndex - 1];

                if (selectedPokemon.Starter || selectedPokemon.Selected)
                {
                    selectedPokemon.Starter = false;
                    selectedPokemon.Selected = false;
                } else if (selectedPokemon.Selected == false)
                {
                    selectedPokemon.Selected = true;
                    selectedPokemon.Starter = false;
                }

                using (var context = new DatabaseContext())
                {
                    context.PokemonMaster.Update(selectedPokemon);
                    context.SaveChanges();
                }
            }
            else if (choice.ToLower().Contains("start"))
            {
                string[] parts = choice.Split(' ');
                if (parts.Length == 2 && int.TryParse(parts[0], out int startIndex) && startIndex > 0 && startIndex <= PokemonList.Count)
                {
                    var selectedPokemon = PokemonList[startIndex - 1];

                    if (selectedPokemon.Starter)
                    {
                        selectedPokemon.Starter = false;
                        selectedPokemon.Selected = true;
                    } 
                    else
                    {
                        selectedPokemon.Selected = true;
                        selectedPokemon.Starter = true;
                    }


                    using (var context = new DatabaseContext())
                    {
                        context.PokemonMaster.Update(selectedPokemon);
                        context.SaveChanges();
                    }
                }
            } else
            {
                continue;
            }
        }
    }
}