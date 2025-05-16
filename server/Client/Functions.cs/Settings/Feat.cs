using Database;
using Models;
using System.Text;

namespace Server;

public class God
{
    public static async Task EnterNewRealm(ClientSession session)
    {
        User user;
        using var context = new DatabaseContext();
        {
            user = context.Users.FirstOrDefault(u => u.Username == session.Username)!;
        }
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
                    while (true)
                    {
                        string pokemonName = await session.GetInputAsync("Enter the name of the Pokémon you want to create (\"cancel\" to leave):");
                        pokemonName = pokemonName.ToLower();

                        if (string.IsNullOrWhiteSpace(pokemonName))
                        {
                            await session.SendMessageAsync("Invalid Pokémon name. Please try again.");
                            continue;
                        }

                        if (!ListofStuff.AllPokemon.Contains(pokemonName.ToLower()))
                        {
                            await session.SendMessageAsync($"The Pokémon {pokemonName} does not exist. Please try again.");
                            continue;
                        }

                        if (pokemonName.ToLower() == "cancel")
                        {
                            break;
                        }

                        string health = await session.GetInputAsync("Enter the Pokémon's health (\"cancel\" to leave):");

                        if (int.TryParse(health, out int healthValue))
                        {
                            if (healthValue < 0)
                            {
                                await session.SendMessageAsync("Health cannot be negative. Please try again.");
                                continue;
                            }
                        }
                        else
                        {
                            await session.SendMessageAsync("Invalid health value. Please try again.");
                            continue;
                        }

                        string exp = await session.GetInputAsync("Enter the Pokémon's Exp (\"cancel\" to leave):");

                        if (int.TryParse(exp, out int expValue))
                        {
                            if (expValue < 0)
                            {
                                await session.SendMessageAsync("Experience cannot be negative. Please try again.");
                                continue;
                            }
                        }
                        else
                        {
                            await session.SendMessageAsync("Invalid experience value. Please try again.");
                            continue;
                        }

                        try
                        {
                            var newPokemon = user.AdminGetPokemon(pokemonName, user.Id!, healthValue, expValue);

                            if (newPokemon == null)
                            {
                                await session.SendMessageAsync("Failed to create Pokémon. Please try again.");
                                continue;
                            }

                            using (var innerContext = new DatabaseContext())
                            {
                                innerContext.PokemonMaster.Add(newPokemon);
                                innerContext.SaveChanges();
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            await session.SendMessageAsync($"Error creating Pokémon: {ex.Message}");
                            Console.WriteLine($"Error creating Pokémon: {ex.Message}");
                            continue;
                        }
                    }

                    break;
                case "2":
                    while (true)
                    {
                        string userName = await session.GetInputAsync("Enter the username of the user you want to ban (\"cancel\" to leave):");
                        userName = userName.ToLower();
                        var userToBan = context.Users.FirstOrDefault(u => u.Username == userName);

                        if (string.IsNullOrWhiteSpace(userName))
                        {
                            await session.SendMessageAsync("Invalid username. Please try again.");
                            continue;
                        }

                        if (userToBan == null)
                        {
                            await session.SendMessageAsync($"The user {userName} does not exist. Please try again.");
                            continue;
                        }

                        if (userName.ToLower() == "cancel")
                        {
                            break;
                        }

                        try
                        {
                            if (userToBan == null)
                            {
                                await session.SendMessageAsync($"Failed to locate user {userName}. Please try again.");
                                continue;
                            }

                            userToBan.IsBanned = !userToBan.IsBanned;
                            
                            if (userToBan.IsBanned)
                            {
                                await session.SendMessageAsync($"User {userName} has been banned.");
                            }
                            else
                            {
                                await session.SendMessageAsync($"User {userName} has been unbanned.");
                            }


                            context.Users.Update(userToBan);
                            context.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            await session.SendMessageAsync($"Error banning user: {ex.Message}");
                            Console.WriteLine($"Error banning user: {ex.Message}");
                            continue;
                        }
                    }
                    break;
                default:
                    await session.SendMessageAsync("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}