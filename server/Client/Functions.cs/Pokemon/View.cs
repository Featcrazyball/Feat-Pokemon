using Database;
using PokemonPocket;
using Models;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Server;

public class ViewPokemonFunc
{
    public static async Task ViewPokemon(ClientSession session, User user)
    {
        // Fetch user's Pokémon from the database with their skills
        var pokemonList = new List<PokemonMaster>();

        using (var context = new DatabaseContext())
        {
            pokemonList = context.PokemonMaster
                .Include(p => p.Skills)
                .Where(p => p.OwnerId == user.Id)
                .OrderByDescending(p => p.Experience)
                .ToList();
        }

        if (pokemonList.Count == 0)
        {
            await session.SendMessageAsync("You have no Pokémon in your collection.");
            return;
        }

        await session.SendMessageAsync("-----------------------------------------\n" +
                                        "Pokémon Collection:\n" +
                                        "-----------------------------------------\n");
        
        // Display Pokémon information with clear numbering
        for (int i = 0; i < pokemonList.Count; i++)
        {
            var pokemon = pokemonList[i];
            int pokemonNumber = i + 1; // Explicitly number each Pokémon
            
            await session.SendMessageAsync(
$"Pokémon Number: {pokemonNumber}\n" +
$"Name: {pokemon.Name}\n" +
$"Nickname: {pokemon.Nickname}\n" +
$"Level: {pokemon.Level}\n" +
$"Experience: {pokemon.Experience}\n" +
$"Type: {pokemon.Type}\n" +
$"HP: {pokemon.MaxHealth}\n" +
$"Attack: {pokemon.MaxAttack}\n" +
$"Defense: {pokemon.MaxDefense}\n" +
$"Sp. Attack: {pokemon.MaxSpecialAttack}\n" +
$"Sp. Defense: {pokemon.MaxSpecialDefense}\n" +
$"Speed: {pokemon.MaxSpeed}\n" +
$"Crit Rate: {Math.Min(pokemon.CritRate * 100, 100f).ToString("F2").TrimEnd('0').TrimEnd('.')}%\n" +
$"Stat Points: {pokemon.StatPoints}\n" +
$"Skills: {string.Join(", ", pokemon.Skills.Select(s => s.Name))}\n"+
$"Evolve Requirements: {pokemon.Requirements}\n" +
"---------\n" +
$"Assignment Skill: {pokemon.Skill}\n" +
$"Assignment Skill Damage: {pokemon.SkillDamage}\n" +
$"-----------------------------------------\n"
);
        }

        await session.SendMessageAsync("Please visit https://pokemondb.net/move/generation/1/ for more information on skills.\n");

        // Display options for user
        await session.SendMessageAsync("Options:\n" +
                                        "[B] Back to Pokémon Menu\n" +
                                        "[N] Nickname Pokémon\n" +
                                        "[P] Allocate Stat Points\n" +
                                        "[D] Damage Calculator\n" +
                                        "[L] Level Up Pokémon\n" +
                                        $"{(user.God ? $"[F] Future Evolutions (God)\n" : "")}\n"
                                        );
            
        // Handle user input for actions


        string userChoice = await session.GetInputAsync("Choice:");

        switch (userChoice.ToUpper())
        {
            case "B":
                // Return to Pokémon menu
                return;
            case "N":
                string pokemonNumber = await session.GetInputAsync("Enter the number of the Pokémon to nickname:");
                if (int.TryParse(pokemonNumber, out int index) && index > 0 && index <= pokemonList.Count)
                {
                    var selectedPokemon = pokemonList[index-1];
                    string newNickname;
                    while (true)
                    {
                        newNickname = await session.GetInputAsync($"Enter a new nickname for {selectedPokemon.Name}:");
                        if (newNickname.Length > 15)
                        {
                            await session.SendMessageAsync("Nickname is too long. Please enter a nickname with 15 characters or less.");
                            continue;
                        } else
                        {
                            break;
                        }
                    }
                    
                    using (var context = new DatabaseContext())
                    {
                        selectedPokemon.Nickname = newNickname;
                        context.PokemonMaster.Update(selectedPokemon);
                        context.SaveChanges();
                    }

                    await session.SendMessageAsync($"{selectedPokemon.Name} is now known as {newNickname}!");
                }
                else
                {
                    await session.SendMessageAsync("Invalid Pokémon number.");
                }
                
                await session.GetInputAsync("Input any key to continue...");

                // Return to view Pokémon after nickname change
                await ViewPokemon(session, user);
                break;

            case "P":
                pokemonNumber = await session.GetInputAsync("Enter the number of the Pokémon to nickname:");
                if (int.TryParse(pokemonNumber, out index) && index > 0 && index <= pokemonList.Count)
                {
                    var selectedPokemon = pokemonList[index-1];
                    await session.SendMessageAsync($"You have selected {selectedPokemon.Name}.");
                    await session.SendMessageAsync($"{selectedPokemon.Name} has {selectedPokemon.StatPoints} stat points to allocate.\n");

                    while (true)
                    {
                        string statChoice = await session.GetInputAsync("Enter the stat to allocate points to (HP, ATK, DEF, SpAtk, SpDef, SPD) or 'done' to finish:");
                        if (statChoice.ToUpper() == "DONE")
                            break;

                        string count = await session.GetInputAsync("\nEnter the number of points to allocate:");

                        int pointsToAllocate = int.Parse(count);
                        try
                        {
                            if (!int.TryParse(count, out pointsToAllocate) || pointsToAllocate <= 0)
                            {
                                await session.SendMessageAsync("Invalid number of points. Please try again.");
                                continue;
                            }
                        }
                        catch (FormatException)
                        {
                            await session.SendMessageAsync("Invalid input.");
                            continue;
                        }
    
                        switch (statChoice.ToUpper())
                        {
                            case "HP":
                                await selectedPokemon.AssignStatPoints(pointsToAllocate, "health", session);
                                break;
                            case "ATK":
                                await selectedPokemon.AssignStatPoints(pointsToAllocate, "attack", session);
                                break;
                            case "DEF":
                                await selectedPokemon.AssignStatPoints(pointsToAllocate, "defense", session);
                                break;
                            case "SPATK":
                                await selectedPokemon.AssignStatPoints(pointsToAllocate, "specialattack", session);
                                break;
                            case "SPDEF":
                                await selectedPokemon.AssignStatPoints(pointsToAllocate, "specialdefense", session);
                                break;
                            case "SPD":
                                await selectedPokemon.AssignStatPoints(pointsToAllocate, "speed", session);
                                break;
                            default:
                                await session.SendMessageAsync("Invalid stat choice.");
                                break;
                        }

                    }

                    using (var context = new DatabaseContext())
                    {
                        context.PokemonMaster.Update(selectedPokemon);
                        context.SaveChanges();
                    }

                    await session.SendMessageAsync($"Stat Points have been allocated successfully!");
                }
                else
                {
                    await session.SendMessageAsync("Invalid Pokémon number.");
                }
                break;

            case "D":
                // Damage calculator
                string attackerNumber = await session.GetInputAsync("Enter the number of the Attacker Pokémon:");
                string defenderNumber = await session.GetInputAsync("Enter the number of the Defender Pokémon:");

                if (int.TryParse(attackerNumber, out int attackerIndex) && int.TryParse(defenderNumber, out int defenderIndex) &&
                    attackerIndex > 0 && attackerIndex <= pokemonList.Count && defenderIndex > 0 && defenderIndex <= pokemonList.Count)
                {
                    var attacker = pokemonList[attackerIndex - 1];
                    var defender = pokemonList[defenderIndex - 1];

                    // Perform damage calculation
                    float damage = defender.calculateDamage(attacker.SkillDamage);

                    await session.SendMessageAsync($"The damage dealt by {attacker.Name} to {defender.Name} is: {damage}");
                    await session.GetInputAsync("Input any key to continue...");
                }
                else
                {
                    await session.SendMessageAsync("Invalid Pokémon numbers.");
                }
                break;
            
            case "L":
                // Level up Pokémon
                string levelUpNumber = await session.GetInputAsync("Enter the number of the Pokémon to level up:");
                if (int.TryParse(levelUpNumber, out index) && index > 0 && index <= pokemonList.Count)
                {
                    var selectedPokemon = pokemonList[index-1];

                    int times = selectedPokemon.Experience / 1000;

                    if (times == 0)
                    {
                        await session.SendMessageAsync($"{selectedPokemon.Name} does not have enough experience to level up.");
                        await session.GetInputAsync("Input any key to continue...");
                        break;
                    }

                    if (times > 100)
                        {
                            times = 100;
                        }

                    await session.SendMessageAsync("------------------------------------------------------------------------------------");
                    await selectedPokemon.LevelUp(times, session);
                    await session.SendMessageAsync("\n------------------------------------------------------------------------------------");

                    using (var context = new DatabaseContext())
                    {
                        context.PokemonMaster.Update(selectedPokemon);
                        context.SaveChanges();
                    }
                }
                else
                {
                    await session.SendMessageAsync("Invalid Pokémon number.");
                }

                await session.GetInputAsync("Input any key to continue...");
                break;

            case "F":
                if (!user.God)
                {
                    break;
                }
                var futureMenu = new StringBuilder();
                var listPoki = new Dictionary<string, int>();
                int evolvableList = 0;

                foreach (var pokemon in pokemonList)
                {
                    if (listPoki.ContainsKey(pokemon.Name!))
                    {
                        listPoki[pokemon.Name!] += 1;
                        continue;
                    }
                    if (pokemon.Requirements != "Unevolvable")
                    {
                        listPoki[pokemon.Name!] = 1;
                        string displayName = pokemon.Nickname == "None" ? pokemon.Name! : pokemon.Nickname!;
                        string evolveName = pokemon.EvolvesTo ?? "Unknown";
                        evolvableList++;
                    }
                }
                
                if (evolvableList == 0)
                {
                    await session.SendMessageAsync("No Pokémon can evolve at this time.");
                    await session.GetInputAsync("Input any key to continue...");
                    break;
                }
                
                var listMenu = new StringBuilder();
                
                listMenu.AppendLine(@"
╔═══════════════════════════════ EVOLUTION MENU ═══════════════════════════════╗
║                                                                              ║
║  🔮 The following Pokémon can currently evolve into the following forms:     ║");

                foreach (var pokemonEntry in listPoki)
                {
                    string pokemonName = pokemonEntry.Key;
                    int count = pokemonEntry.Value;

                    var pokemonInfo = pokemonList.FirstOrDefault(p => p.Name == pokemonName);

                    if (pokemonInfo != null)
                    {
                        bool Evolvable = pokemonInfo.Requirements != "Unevolvable";
                        if (!Evolvable) {
                            continue;
                        }

                        string displayName = $"{count} {pokemonName}";
                        string evolveName = pokemonInfo.EvolvesTo ?? "Unknown";

                        listMenu.AppendLine($"║                                                                              ║");
                        listMenu.AppendLine($"║  {displayName.PadRight(18)} --> Evolves to: {evolveName.PadRight(25)}  ║             ║");
                    }
                }
                listMenu.AppendLine($"║                                                                              ║");
                listMenu.AppendLine($"║  💡 Visit the shop to purchase evolutionary stones                           ║");
                listMenu.AppendLine($"║                                                                              ║");
                listMenu.AppendLine($"╚══════════════════════════════════════════════════════════════════════════════╝");

                await session.SendMessageAsync(listMenu.ToString());

                string evolveChoice = await session.GetInputAsync("Would you like to evolve ALL the pokemon in the list? (Assignment) (Y/N):");

                if (evolveChoice.ToUpper() == "Y")
                {
                    List<string> evolvablePokemonIds = new List<string>();
                    foreach (var pokemonEntry in pokemonList)
                    {
                        bool Evolvable = pokemonEntry.Requirements != "Unevolvable";
                        if (Evolvable)
                        {
                            evolvablePokemonIds.Add(pokemonEntry.Id!);
                        }
                    }

                    foreach (var pokemonId in evolvablePokemonIds)
                    {
                        PokemonMaster pokemonToEvolve;
                        using (var context = new DatabaseContext())
                        {
                            pokemonToEvolve = context.PokemonMaster.FirstOrDefault(p => p.Id == pokemonId)!;
                        }

                        if (pokemonToEvolve == null)
                        {
                            await session.SendMessageAsync($"Pokémon with ID {pokemonId} not found.");
                            continue;
                        }

                        try
                        {
                            await pokemonToEvolve.GodEvolve(session);
                            pokemonToEvolve.ForgetTillFive();
                        } 
                        catch (Exception ex)
                        {
                            await session.SendMessageAsync($"Error evolving Pokémon with ID {pokemonId}: {ex.Message}");
                            continue;
                        }
                    }
                }
                else
                {
                    await session.SendMessageAsync("No Pokémon were evolved.");
                }

                await session.GetInputAsync("Input any key to continue...");
                break;

            default:
                await session.SendMessageAsync("Invalid choice. Returning to main menu...");
                await session.GetInputAsync("Input any key to continue...");
                break;
        }

    }
}
