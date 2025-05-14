using System;
using System.Text;
using Models;
using Database;
using PokemonPocket;
using Server;

namespace Server;

public class ShopBuy
{
    public static async Task BuyItem(ClientSession session)
    {
        User user;

        using (var context = new DatabaseContext())
        {
            user = context.Users.FirstOrDefault(u => u.Username == session.Username)!;
            if (user == null)
            {
                await session.SendMessageAsync("User not found.");
                return;
            }
        }

        while (true)
        {
            await session.SendMessageAsync(@"
    ╔══════════════════════════════════════════════════════════════════════════════╗
    ║                                                                              ║
    ║      ✨✨✨✨✨✨✨✨✨    SHOP MENU    ✨✨✨✨✨✨✨✨✨                   ║
    ║                                                                              ║
    ╠══════════════════════════════════════════════════════════════════════════════╣
    ║    [1] ⚪ POKÉBALL       │ 100 Coins                                         ║
    ║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║
    ║    [2] 🔥 Fire Stone     │ 300 Coins                                         ║
    ║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║
    ║    [3] 💧 Water Stone    │ 300 Coins                                         ║
    ║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║
    ║    [4] 🍃 Leaf Stone     │ 300 Coins                                         ║
    ║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║
    ║    [5] ⚡ Thunder Stone  │ 300 Coins                                         ║
    ║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║
    ║    [6] 🌙 Moon Stone     │ 300 Coins                                         ║
    ║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║
    ║    [7] ⭐ XP Bottle      │ 500 Coins (1000xp/bottle)                         ║
    ║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║
    ║    [8] 💰 BACK           │ Return to Trainer Menu                            ║
    ║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║
    ║                                                                              ║
    ╚══════════════════════════════════════════════════════════════════════════════╝
    ");

            string choice = await session.GetInputAsync("Choice:");

            switch (choice)
            {
                case "1":
                    await session.SendMessageAsync("\n----------------------------------------------------------------------\n");

                    using (var context = new DatabaseContext())
                    {

                        if (user.Coins < 100)
                        {
                            await session.SendMessageAsync("You do not have enough coins to purchase this item.\n");
                            await session.GetInputAsync("Input any key to continue...");
                            continue;
                        }
                        else
                        {
                            user.Coins -= 100;
                            context.Users.Update(user);

                            Random random = new Random();
                            int randomIndex = random.Next(0, ListofStuff.AllPokemon.Count());
                            
                            var pokemon = user.GetPokemonWithoutSaving(ListofStuff.AllPokemon[randomIndex], user.Id!);
                            if (pokemon != null)
                            {
                                context.PokemonMaster.Add(pokemon);

                                foreach (var skill in pokemon.Skills)
                                {
                                    context.Skills.Add(skill);
                                }
                            }
                            await session.SendMessageAsync("You have purchased a Pokéball for 100 coins.\n");
                            await session.SendMessageAsync($"Your Pokéball hatched into a {pokemon!.Name}!\n");
                            context.SaveChanges();
                        }
                    }

                    await session.SendMessageAsync("----------------------------------------------------------------------\n");
                    await session.GetInputAsync("Input any key to continue...");

                    continue;
                case "2":
                    await session.SendMessageAsync("\n----------------------------------------------------------------------");

                    using (var context = new DatabaseContext())
                    {
                        if (user.Coins < 300)
                        {
                            await session.SendMessageAsync("You do not have enough coins to purchase this item.");
                            await session.GetInputAsync("Input any key to continue...");
                            continue;
                        }
                        else
                        {
                            user.Coins -= 300;

                            context.Users.Update(user);
                            context.SaveChanges();
                            Item.AddFireStone(user.Id!, 1);
                            await session.SendMessageAsync("You have purchased a Fire Stone for 300 coins.");
                        }
                    }
                    await session.SendMessageAsync("-------------------------------------------------------------------\n");
                    await session.GetInputAsync("Input any key to continue...");
                    continue;
                case "3":
                    await session.SendMessageAsync("\n----------------------------------------------------------------------");
                    using (var context = new DatabaseContext())
                    {
                        if (user.Coins < 300)
                        {
                            await session.SendMessageAsync("You do not have enough coins to purchase this item.");
                            await session.GetInputAsync("Input any key to continue...");
                            continue;
                        }
                        else
                        {
                            user.Coins -= 300;

                            context.Users.Update(user);
                            context.SaveChanges();
                            Item.AddWaterStone(user.Id!, 1);
                            await session.SendMessageAsync("You have purchased a Water Stone for 300 coins.");
                        }
                    }
                    await session.SendMessageAsync("----------------------------------------------------------------------\n");
                    await session.GetInputAsync("Input any key to continue...");
                    continue;
                case "4":
                    await session.SendMessageAsync("\n----------------------------------------------------------------------");
                    using (var context = new DatabaseContext())
                    {
                        if (user.Coins < 300)
                        {
                            await session.SendMessageAsync("You do not have enough coins to purchase this item.");
                            await session.GetInputAsync("Input any key to continue...");
                            continue;
                        }
                        else
                        {
                            user.Coins -= 300;

                            context.Users.Update(user);
                            context.SaveChanges();
                            Item.AddLeafStone(user.Id!, 1);
                            await session.SendMessageAsync("You have purchased a Leaf Stone for 300 coins.");
                        }
                    }
                    await session.SendMessageAsync("----------------------------------------------------------------------\n");
                    await session.GetInputAsync("Input any key to continue...");

                    continue;
                case "5":
                    await session.SendMessageAsync("\n----------------------------------------------------------------------");

                    using (var context = new DatabaseContext())
                    {
                        if (user.Coins < 300)
                        {
                            await session.SendMessageAsync("You do not have enough coins to purchase this item.");
                            await session.GetInputAsync("Input any key to continue...");
                            continue;
                        }
                        else
                        {
                            user.Coins -= 300;

                            context.Users.Update(user);
                            context.SaveChanges();
                            Item.AddThunderStone(user.Id!, 1);
                            await session.SendMessageAsync("You have purchased a Thunder Stone for 300 coins.");
                        }
                    }
                    await session.SendMessageAsync("----------------------------------------------------------------------\n");
                    await session.GetInputAsync("Input any key to continue...");
                    continue;
                case "6":
                    await session.SendMessageAsync("\n----------------------------------------------------------------------");
                    using (var context = new DatabaseContext())
                    {

                        if (user.Coins < 300)
                        {
                            await session.SendMessageAsync("You do not have enough coins to purchase this item.");
                            await session.GetInputAsync("Input any key to continue...");
                            continue;
                        }
                        else
                        {
                            user.Coins -= 300;

                            context.Users.Update(user);
                            context.SaveChanges();
                            Item.AddMoonStone(user.Id!, 1);
                            await session.SendMessageAsync("You have purchased a Moon Stone for 300 coins.");
                        }
                    }
                    await session.SendMessageAsync("----------------------------------------------------------------------\n");
                    await session.GetInputAsync("Input any key to continue...");
                    continue;
                case "7":
                    // Check if user dont even have enough coins to buy 1
                    using (var context = new DatabaseContext())
                    {
                        if (user.Coins < 500)
                        {
                            await session.SendMessageAsync("You do not have enough coins to purchase this item.");
                            await session.GetInputAsync("Input any key to continue...");
                            continue;
                        }
                    }

                    var UserPokemonList = new List<PokemonMaster>();
                    string number;
                    int parsedNumber;
                    while (true)
                    {
                        try
                        {
                            number = await session.GetInputAsync("How many XP Bottles would you like to purchase?: ");
                            parsedNumber = int.Parse(number);

                            using (var context = new DatabaseContext())
                            {
                                if (user == null)
                                {
                                    await session.SendMessageAsync("User not found.");
                                    return;
                                }

                                if (user.Coins < 500 * parsedNumber)
                                {
                                    await session.SendMessageAsync("You do not have enough coins to purchase this item.");
                                    break;
                                }
                                else
                                {
                                    UserPokemonList = context.PokemonMaster
                                        .Where(p => p.OwnerId == user.Id)
                                        .OrderBy(p => p.Name)
                                        .ToList();
                                    break;
                                }
                            }

                        } catch (FormatException) {
                            await session.SendMessageAsync("Invalid input. Please enter a number.");
                            continue;
                        }
                    }

                    await session.SendMessageAsync("╔══════════════════════════════════════════════════════════════════════════════╗");
                    await session.SendMessageAsync("║                              YOUR POKÉMON                                    ║");
                    await session.SendMessageAsync("╠══════════════════════════════════════════════════════════════════════════════╣");
                    await session.SendMessageAsync("║   #   │ Name                          │ Level   │ XP                         ║");
                    await session.SendMessageAsync("╠═══════╪═══════════════════════════════╪═════════╪════════════════════════════╣");

                    for (int i = 0; i < UserPokemonList.Count; i++)
                    {
                        var pokemon = UserPokemonList[i];
                        var displayName = pokemon.Nickname == "None" ? pokemon.Name : pokemon.Nickname;
                        await session.SendMessageAsync($"║   {i + 1,-3} │ {displayName,-30} │ {pokemon.Level,-7} │ {pokemon.Experience,-25} ║");
                    }

                    await session.SendMessageAsync("╚══════════════════════════════════════════════════════════════════════════════╝");
                    

                    while (true)
                    {
                        using var context = new DatabaseContext();

                        string Pokechoice = await session.GetInputAsync("Choose a Pokémon to use the XP Bottle on: ");
                        if (int.TryParse(Pokechoice, out int index) && index > 0 && index <= UserPokemonList.Count)
                        {
                            var selectedPokemon = UserPokemonList[index - 1];
                            selectedPokemon.Experience += parsedNumber * 1000;
                            var displayName = selectedPokemon.Nickname == "None" ? selectedPokemon.Name : selectedPokemon.Nickname;
                            await session.SendMessageAsync("\n----------------------------------------------------------------------\n");
                            await session.SendMessageAsync($"You have used {parsedNumber} XP Bottles on {displayName}.\n");
                            await session.SendMessageAsync("----------------------------------------------------------------------\n");

                            // Update the user's coins
                            user.Coins -= 500 * parsedNumber;
                            context.Users.Update(user);
                            context.PokemonMaster.Update(selectedPokemon);
                            context.SaveChanges();

                            break;
                        }
                        else
                        {
                            await session.SendMessageAsync("Invalid choice. Please try again.");
                        }
                    }
                    await session.GetInputAsync("Input any key to continue...");

                    continue;
                case "8":
                    await session.SendMessageAsync("Returning to Trainer Menu...");
                    return; // Exit the shop menu
                default:
                    continue;
            }
        }
    }
}