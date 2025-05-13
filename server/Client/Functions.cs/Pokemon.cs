using Server;
using Database;
using Models;
using PokemonPocket;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Server;

public class ServerPokemon
{
    public static async Task PokemonMenu(ClientSession session)
    {
        User user;
        using (var context = new DatabaseContext())
        {
            user = context.Users.FirstOrDefault(u => u.Username == session.Username)!;
        };
        

        if (user == null)
        {
            await session.SendMessageAsync("There has been an error locating your account. Please try again.");
            return;
        }

        await session.SendMessageAsync(@"
╔══════════════════════════════════════════════════════════════════════════════╗
║                                                                              ║
║      ✨✨✨✨✨✨✨✨✨    POKÉMON MENU    ✨✨✨✨✨✨✨✨✨                ║    
║                                                                              ║
╠══════════════════════════════════════════════════════════════════════════════╣
║                                                                              ║
║    [1] 🔥 VIEW   │ View Your Pokémon Collection                              ║
║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║
║    [2] 💰 EVOLVE │ Evolve All Your Pokémon                                   ║
║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║
║    [3] 💬 BACK   │ Return to Trainer Menu                                    ║
║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║
║                                                                              ║
╚══════════════════════════════════════════════════════════════════════════════╝
");
        string choice = await session.GetInputAsync("Choice:");

        switch (choice)
        {
            case "1":
                // View Pokémon
                await ViewPokemonFunc.ViewPokemon(session, user);
                break;
            case "2":
                // Evolve Pokémon
                await EvolvePokemon(session, user);
                break;
            case "3":
                // Back to Trainer Menu
                return;
            case "god mode":
                if (user.God) { await PokemonMax(session, user); } 
                using (var context = new DatabaseContext())
                {
                    context.PokemonMaster
                        .Where(p => p.OwnerId == user.Id)
                        .ToList()
                        .ForEach(p => p.Level = 100);
                    context.PokemonMaster.UpdateRange(context.PokemonMaster);
                    Item.AddFireStone(user.Id!, 100);
                    Item.AddWaterStone(user.Id!, 100);
                    Item.AddThunderStone(user.Id!, 100);
                    Item.AddLeafStone(user.Id!, 100);
                    Item.AddMoonStone(user.Id!, 100);
                    context.SaveChanges();
                }

                break;
            case "test":
            if (user.God) { await PokemonMax(session, user); } 
                using (var context = new DatabaseContext())
                {
                    context.PokemonMaster
                        .Where(p => p.OwnerId == user.Id)
                        .ToList()
                        .ForEach(p => p.Level = 100);
                    context.PokemonMaster.UpdateRange(context.PokemonMaster);
                    context.SaveChanges();
                }
                break;
            default:
                await session.SendMessageAsync("Invalid choice. Please try again.");
                break;
        }
    }

    private static async Task PokemonMax(ClientSession session, User user)
    {
        // Fetch user's Pokémon from the database
        using var context = new DatabaseContext();
        var pokemonList = context.PokemonMaster
            .Where(p => p.OwnerId == user.Id)
            .ToList();

        if (pokemonList.Count == 0)
        {
            await session.SendMessageAsync("You have no Pokémon in your collection.");
            return;
        }

        foreach (var pokemon in pokemonList)
        {
            pokemon.Level = 100;
            await session.SendMessageAsync($"Your {pokemon.Name} has been leveled up to 100!\n");
        }
        context.SaveChanges();
    }

    private static async Task EvolvePokemon(ClientSession session, User user)
    {
        // Fetch user's Pokémon from the database
        using var context = new DatabaseContext();
        var pokemonList = context.PokemonMaster
            .Where(p => p.OwnerId == user.Id)
            .ToList();

        if (!pokemonList.Any())
        {
            await session.SendMessageAsync("You have no Pokémon to evolve.");
            return;
        }

        string evolvable;
        var levelEvolve = new List<PokemonMaster>();
        var itemEvolve = new Dictionary<PokemonMaster, string>();
        var tradeEvolve = new List<PokemonMaster>();
        string item;

        // Assign pokemon numbers for reference
        Dictionary<PokemonMaster, int> pokemonNumbers = new Dictionary<PokemonMaster, int>();
        for (int i = 0; i < pokemonList.Count; i++)
        {
            pokemonNumbers[pokemonList[i]] = i + 1;
        }

        foreach (var pokemon in pokemonList)
        {
            try
            {
                evolvable = pokemon.CheckEvolve();
                
                // More lenient parsing that's less dependent on exact format
                if (evolvable.StartsWith("false"))
                {
                    continue;
                }
                else if (evolvable.StartsWith("true"))
                {
                    // Level evolution
                    if (evolvable.Contains("level"))
                    {
                        levelEvolve.Add(pokemon);
                    }
                    // Item evolution - looking for any mention of "item" and a pipe separator
                    else if (evolvable.Contains("item") && evolvable.Contains("|"))
                    {
                        string[] itemParts = evolvable.Split("|");
                        if (itemParts.Length >= 2)
                        {
                            item = itemParts[1].Trim();
                            itemEvolve.Add(pokemon, item);
                        }
                    }
                    // Trade evolution
                    else if (evolvable.Contains("trade"))
                    {
                        tradeEvolve.Add(pokemon);
                    }
                }
            }
            catch (Exception ex)
            {
                // More detailed error information
                await session.SendMessageAsync($"Error parsing evolution for {pokemon.Name}: {ex.Message}");
                continue;
            }
        }

        if (!levelEvolve.Any() && !itemEvolve.Any() && !tradeEvolve.Any())
        {
            await session.SendMessageAsync("None of your Pokémon are ready to evolve.");
            return;
        }

        // Handle evolution logic for each category (level, item, trade)
        if (levelEvolve.Any())
        {
            var levelMenu = new StringBuilder();
            levelMenu.AppendLine(@"
╔═════════════════════════════════ LEVEL EVOLUTION ══════════════════════════════╗
║                                                                                ║
║  ✨ The following Pokémon have reached level requirements for evolution:       ║");

            foreach (var pokemon in levelEvolve)
            {
                string displayName = pokemon.Nickname == "None" ? pokemon.Name! : pokemon.Nickname!;
                int level = pokemon.Level;
                string type = pokemon.Type ?? "Unknown";
                int pokemonNumber = pokemonNumbers[pokemon];
                levelMenu.AppendLine($"║                                                                                ║");
            levelMenu.AppendLine($"║  🔆 {displayName.PadRight(16)} | Lvl: {level.ToString().PadRight(3)} | Type: {type.PadRight(15)}  ║                     ║");
            }
            
            levelMenu.AppendLine($"║                                                                                ║");
            levelMenu.AppendLine($"╚════════════════════════════════════════════════════════════════════════════════╝");
            
            await session.SendMessageAsync(levelMenu.ToString());
        }

        // Item-based evolution menu
        if (itemEvolve.Any())
        {
            var itemMenu = new StringBuilder();
            itemMenu.AppendLine(@"
╔══════════════════════════════ STONE EVOLUTION ═══════════════════════════════╗
║                                                                              ║
║  🔮 The following Pokémon can evolve using evolutionary stones:              ║");

            foreach (var pokemon in itemEvolve)
            {
                string displayName = pokemon.Key.Nickname == "None" ? pokemon.Key.Name! : pokemon.Key.Nickname!;
                string requiredItem = pokemon.Value;
                int pokemonNumber = pokemonNumbers[pokemon.Key];
                itemMenu.AppendLine($"║                                                                              ║");
                itemMenu.AppendLine($"║  [{pokemonNumber}] {displayName.PadRight(18)} | Requires: {requiredItem.PadRight(15)}  ║                       ║");
            }
            
            itemMenu.AppendLine($"║                                                                              ║");
            itemMenu.AppendLine($"║  💡 Visit the shop to purchase evolutionary stones                           ║");
            itemMenu.AppendLine($"╚══════════════════════════════════════════════════════════════════════════════╝");
            
            await session.SendMessageAsync(itemMenu.ToString());
        }

        // Trade-based evolution menu
        if (tradeEvolve.Any())
        {
            var tradeMenu = new StringBuilder();
            tradeMenu.AppendLine(@"
╔═════════════════════════════ TRADE EVOLUTION ══════════════════════════════╗
║                                                                            ║
║  🔄 The following Pokémon evolve through trading:                          ║");

            foreach (var pokemon in tradeEvolve)
            {
                string displayName = pokemon.Nickname == "None" ? pokemon.Name! : pokemon.Nickname!;
                int level = pokemon.Level;
                int pokemonNumber = pokemonNumbers[pokemon];
                tradeMenu.AppendLine($"║                                                                            ║");
                tradeMenu.AppendLine($"║  [{pokemonNumber}] {displayName.PadRight(18)} | Current Level: {level.ToString().PadRight(3)}                  ║");
            }
            
            tradeMenu.AppendLine($"║                                                                            ║");
            tradeMenu.AppendLine($"║  💡 Visit the trading center to evolve these Pokémon                       ║");
            tradeMenu.AppendLine($"╚════════════════════════════════════════════════════════════════════════════╝");
            
            await session.SendMessageAsync(tradeMenu.ToString());
        }

        // Ask user for evolve action
        var evolutionMenu = new StringBuilder();
        evolutionMenu.AppendLine("╔══════════════════════════════════════════════════════════════════════════════╗");
        evolutionMenu.AppendLine("║                                                                              ║");
        evolutionMenu.AppendLine("║      ✨✨✨✨✨✨✨✨✨    EVOLUTION MENU    ✨✨✨✨✨✨✨✨✨              ║");
        evolutionMenu.AppendLine("║                                                                              ║");
        evolutionMenu.AppendLine("╠══════════════════════════════════════════════════════════════════════════════╣");
        evolutionMenu.AppendLine("║                                                                              ║");
        evolutionMenu.AppendLine("║    [1] 🔥 EVOLVE │ Evolve Pokémon Using Level Up                             ║");
        evolutionMenu.AppendLine("║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║");
        evolutionMenu.AppendLine("║    [2] 💰 ITEM   │ Evolve Pokémon Using Items                                ║");
        evolutionMenu.AppendLine("║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║");
        evolutionMenu.AppendLine("║    [3] 💬 TRADE  │ Evolve Pokémon Using Trade (100 Coins)                    ║");
        evolutionMenu.AppendLine("║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║");
        evolutionMenu.AppendLine("║    [4] 💬 BACK   │ Return to Pokémon Menu                                    ║");
        evolutionMenu.AppendLine("║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║");
        evolutionMenu.AppendLine("║                                                                              ║");
        evolutionMenu.AppendLine("╚══════════════════════════════════════════════════════════════════════════════╝");

        await session.SendMessageAsync(evolutionMenu.ToString());
        string choice = await session.GetInputAsync("Choice:");

        // Handle evolution based on user's choice
        switch (choice)
        {
            case "1":
                if (levelEvolve.Any())
                {
                    // Store the IDs of Pokémon to evolve, not the entities themselves
                    var pokemonIdsToEvolve = levelEvolve.Select(p => p.Id).ToList();
                    
                    // Dispose the original context
                    context.Dispose();
                    
                    // Process each Pokémon evolution separately
                    foreach (var pokemonId in pokemonIdsToEvolve)
                    {
                        try
                        {
                            // Create a fresh context for each evolution
                            using (var evolutionContext = new DatabaseContext())
                            {
                                // Find the Pokémon using its ID
                                var freshPokemon = evolutionContext.PokemonMaster.Find(pokemonId);
                                
                                if (freshPokemon != null)
                                {
                                    // Track just the evolution, and let the context dispose
                                    string pokemonName = freshPokemon.Name!;
                                    await session.SendMessageAsync("------------------------------------------------------");
                                    await freshPokemon.Evolve(session);
                                    freshPokemon.ForgetTillFive();
                                    await session.SendMessageAsync("------------------------------------------------------\n");
                                    context.SaveChanges();
                                }
                            } // Context is disposed here

                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                    
                    await session.SendMessageAsync("Evolution process complete!");
                    await session.GetInputAsync("\nInput any key to continue...");
                }
                else
                {
                    await session.SendMessageAsync("No Pokémon are ready for level-based evolution.");
                }
                break;
            case "2":
                if (itemEvolve.Any())
                {
                    string pokemonNumber = await session.GetInputAsync("Enter the number of the Pokémon to evolve:");
                    
                    // Close original context
                    context.Dispose();
                    
                    if (int.TryParse(pokemonNumber, out int pokemonId))
                    {
                        var foundPair = itemEvolve.FirstOrDefault(pair => pokemonNumbers[pair.Key] == pokemonId);
                        
                        if (foundPair.Key != null)
                        {
                            // Get original info before creating new context
                            string pokemonDbId = foundPair.Key.Id!;
                            
                            // Use a fresh context for evolution
                            using (var freshContext = new DatabaseContext())
                            {
                                var freshPokemon = freshContext.PokemonMaster.Find(pokemonDbId);
                                
                                if (freshPokemon != null)
                                {
                                    await session.SendMessageAsync("------------------------------------------------------");
                                    await freshPokemon.Evolve(session);
                                    freshPokemon.ForgetTillFive();
                                    await session.SendMessageAsync("------------------------------------------------------\n");
                                    await session.GetInputAsync("Input any key to continue...");
                                }
                                else
                                {
                                    await session.SendMessageAsync("Could not find the selected Pokémon in the database.");
                                }
                            }
                        }
                        else
                        {
                            await session.SendMessageAsync("Invalid Pokémon number.");
                        }
                    }
                    else
                    {
                        await session.SendMessageAsync("Invalid input. Please enter a number.");
                    }
                }
                else
                {
                    await session.SendMessageAsync("No Pokémon are ready for item-based evolution.");
                }
                break;
            case "3":
                if (itemEvolve.Any())
                {
                    string pokemonNumber = await session.GetInputAsync("Enter the number of the Pokémon to evolve:");
                    
                    // Close original context
                    context.Dispose();
                    
                    if (int.TryParse(pokemonNumber, out int pokemonId))
                    {
                        var foundPair = tradeEvolve.FirstOrDefault(pair => pokemonNumbers[pair] == pokemonId);
                        
                        if (foundPair != null)
                        {
                            string pokemonDbId = foundPair.Id!;
                            
                            // Use a fresh context for evolution
                            using (var freshContext = new DatabaseContext())
                            {
                                var freshPokemon = freshContext.PokemonMaster.Find(pokemonDbId);
                                
                                var UserEvolve = freshContext.Users.FirstOrDefault(u => u.Id == user.Id);
                                if (UserEvolve != null && freshPokemon != null)
                                {
                                    UserEvolve.Coins -= 100;
                                    freshContext.Users.Update(UserEvolve);
                                    freshContext.SaveChanges();
                                    await session.SendMessageAsync("------------------------------------------------------");
                                    await freshPokemon.Evolve(session);
                                    freshPokemon.ForgetTillFive();
                                    await session.SendMessageAsync("------------------------------------------------------\n");
                                    
                                    await session.GetInputAsync("Input any key to continue...");
                                }
                                else
                                {
                                    await session.SendMessageAsync("Could not find the selected Pokémon in the database.");
                                }
                            }
                        }
                        else
                        {
                            await session.SendMessageAsync("Invalid Pokémon number.");
                        }
                    }
                    else
                    {
                        await session.SendMessageAsync("Invalid input. Please enter a number.");
                    }
                }
                else
                {
                    await session.SendMessageAsync("No Pokémon are ready for item-based evolution.");
                }
                break;
            case "4":
                // Return to Pokémon Menu
                return;
            default:
                await session.SendMessageAsync("Invalid choice.");
                break;
        }
    }
}