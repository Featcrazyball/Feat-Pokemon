using Server;
using PokemonPocket;
using Models;
using System.Text;
using Database;
using System.Threading.Tasks;

namespace Arena;

// Make it so that if Bide is still being used, lock it so cant use untill ends.
// Check if bind if in effect, if in effect, cannot change pokemon
// Check if flinch, cannopt move for THAT TURN (reset every turn)

// When a Pokémon is paralyzed, it has a chance to be unable to act during its turn. Specifically, there is a 25% chance that a paralyzed Pokémon will be unable to move and will lose its turn.
// When first turn, bind the skill used to pokemon for Conversion
// Reminder for Dig, it will not hit if the opponent is underground, so check if the opponent is underground before applying damage
// Reminder for Confusion, if the target is confused, it has a 50% chance to hit itself in confusion. If it does, it will take damage equal to 40% of its max health. The confusion lasts for 2-5 turns.
// Reminder for disable, it will disable the last move used by the target for 2-5 turns. The target will not be able to use that move during that time. If the target tries to use the disabled
// Reminder for dig to fix modifeier
// Reminder for burn, it will reduce the attack of the target by 50% and will deal damage equal to 1/16 of its max health at the end of each turn. Switching out does not solve burning
// Reminder for freozen, it will prevent the target from moving. The frozen status can be removed by using certain moves or items, or by switching out the Pokémon. The frozen status lasts until the target is thawed or switched out.
// Reminder for Fly, for their priority next turn
// Reminder to make a function to check attributes, cuz paralyze and burn reduce and stage attribute changes. this will occur after skill is used for both players
// Reminder for Hyper Beam, it will require a recharge turn unless it defeats the target. If the target is knocked out, the user will not need to recharge. The recharge turn will be skipped if the target is knocked out.
// Reminder for Sleep, it will prevent the target from moving. The sleep status can be removed by using certain moves or items, or by switching out the Pokémon. The sleep status lasts until the target is healed or switched out.
// Reminder for light screen to reduce turns and check if it is active
// Reminder for leech seed to check if it is active and reduce turns
// Reminder to lower Mist turns every turn
// Reminder for Mimic, can only be used once
// Reminder to delete metronome skill after battle
// Reminder to add coins from payday after battle
// Reminder for Petal Dance, it will hit 2-5 times in a row. The user will become confused after the last hit. The confusion lasts for 2-5 turns. The confusion can be removed by using certain moves or items, or by switching out the Pokémon. The confusion lasts until the target is healed or switched out.
// Reminder for Poison, it will reduce the health of the target by 1/16 of its max health at the end of each turn. Switching out does not solve poison
// Reminder for Rage, turn it off
// Reminder for sleep, confusion, paralysis, and freeze, they cure when swicthed out
// Reminder for razor wind, it charge on first, and then can attk on 2nd or 3rd or wtv
// Reminder for Rest, it will heal the user to full health and remove all status conditions. The user will fall asleep for 2 turns. The sleep status can be removed by using certain moves or items, or by switching out the Pokémon. The sleep status lasts until the target is healed or switched out.
// Rest clears burn, paralysis, freeze, and poison. It does not clear confusion or sleep. The sleep status can be removed by using certain moves or items, or by switching out the Pokémon. The sleep status lasts until the target is healed or switched out.
// Reminder sleep does not cure when switched out
// Reminder for roar to switch out pokemon if lower level
// Reminder for struggle to only be used if no pp left
// Reminder for thrash, it will hit 2-3 times in a row. The user will become confused after the last hit. The confusion lasts for 2-5 turns. The confusion can be removed by using certain moves or items, or by switching out the Pokémon. The confusion lasts until the target is healed or switched out.
// Reminder to undo conversion after battle to original type
// reminder for badly poisoned, the turns increase by 1 every turn. The damage is calculated as 1/16 of the target's max health + 1/16 of the target's max health * turns. The damage is applied at the end of each turn. The badly poisoned status can be removed by using certain moves or items, or by switching out the Pokémon. The badly poisoned status lasts until the target is healed or switched out.
// Reminder for transform to return everything to normal after battle
// Reminder for transform that it returns to original form after battle
// Reminder for transform to use skills of target
// Reminder for whrilwind to switch out pokemon

public class Arena
{
    public User? creator { get; set; }
    public User? joiner { get; set; }

    // Creator Pokemon
    public virtual ICollection<PokemonMaster>? creatorPokemon { get; set; } = new List<PokemonMaster>();
    public PokemonMaster? CreatorBattle { get; set; } = null;
    public virtual ICollection<PokemonMaster>? creatorFainted { get; set; } = new List<PokemonMaster>();

    // Joiner Pokemon
    public virtual ICollection<PokemonMaster>? joinerPokemon { get; set; } = new List<PokemonMaster>();
    public PokemonMaster? JoinerBattle { get; set; } = null;
    public virtual ICollection<PokemonMaster>? joinerFainted { get; set; } = new List<PokemonMaster>();

    // Sessions
    public ClientSession CreatorSession { get; set; }
    public ClientSession JoinerSession { get; set; }

    // Battle Stats
    public int turn { get; set; } = 0;

    // Response
    public bool creatorResponse { get; set; } = false;
    public bool joinerResponse { get; set; } = false;

    private PokemonBackupService _backupService = new PokemonBackupService();

    public Arena(User player1, User player2, ClientSession session1, ClientSession session2)
    {
        // Players
        CreatorSession = session1;
        JoinerSession = session2;

        creator = player1;
        joiner = player2;

    }

    // Very important for killing pokemon and ending batle
    public async Task<bool?> CheckStats(ClientSession creator, ClientSession joiner)
    {
        if (creatorPokemon == null) { await creator.SendMessageAsync("You have no Pokemon!"); return null; }
        if (joinerPokemon == null) { await joiner.SendMessageAsync("You have no Pokemon!"); return null; }
        if (CreatorBattle == null) { await creator.SendMessageAsync("You have no Pokemon!"); return null; }
        if (JoinerBattle == null) { await joiner.SendMessageAsync("You have no Pokemon!"); return null; }
        if (creatorFainted == null) creatorFainted = new List<PokemonMaster>();
        if (joinerFainted == null) joinerFainted = new List<PokemonMaster>();

        foreach (var pokemon in creatorPokemon)
        {
            if (pokemon.Health > pokemon.MaxHealth) pokemon.Health = pokemon.MaxHealth;
        }

        foreach (var pokemon in joinerPokemon)
        {
            if (pokemon.Health > pokemon.MaxHealth) pokemon.Health = pokemon.MaxHealth;
        }

        if (CreatorBattle.Health > CreatorBattle.MaxHealth) CreatorBattle.Health = CreatorBattle.MaxHealth;
        if (JoinerBattle.Health > JoinerBattle.MaxHealth) JoinerBattle.Health = JoinerBattle.MaxHealth;

        // Check for fainted Pokemon
        if (CreatorBattle.Health <= 0)
        {
            await creator.SendMessageAsync($"Your {CreatorBattle.Name} has fainted!");
            await joiner.SendMessageAsync($"{creator.Username} {CreatorBattle.Name} has fainted!");

            // print out all remaining pokemon
            if (creatorPokemon.Count > 0)
            {
                var creatorTask = Task.Run(async () =>
                {
                    await FaintSwitchPokemon(creator, "creator");
                });

                var joinerTask = Task.Run(async () =>
                {
                    await joiner.SendMessageAsync("Please wait for your opponent to choose their next Pokémon.");
                });

                await Task.WhenAll(creatorTask, joinerTask);
                await creator.SendMessageAsync($"You have switched to {CreatorBattle.Name}.");
                await joiner.SendMessageAsync($"{creator.Username} has switched to {CreatorBattle.Name}.");
            }
            else
            {
                return true;
            }
        }

        if (JoinerBattle.Health <= 0)
        {
            await joiner.SendMessageAsync($"Your {JoinerBattle.Name} has fainted!");
            await creator.SendMessageAsync($"{joiner.Username} {JoinerBattle.Name} has fainted!");

            // print out all remaining pokemon
            if (joinerPokemon.Count > 0)
            {
                var joinerTask = Task.Run(async () =>
                {
                    await FaintSwitchPokemon(joiner, "joiner");
                });

                var creatorTask = Task.Run(async () =>
                {
                    await creator.SendMessageAsync("Please wait for your opponent to choose their next Pokémon.");
                });

                await Task.WhenAll(joinerTask, creatorTask);
                await joiner.SendMessageAsync($"You have switched to {JoinerBattle.Name}.");
                await creator.SendMessageAsync($"{joiner.Username} has switched to {JoinerBattle.Name}.");
            }
            else
            {
                return false;
            }
        }

        return CheckWinner();
    }

    public async Task StartTurn(ClientSession creator, ClientSession joiner)
    {
        // Change this
        if (creatorPokemon == null) { await creator.SendMessageAsync("You have no Pokemon!"); return; }
        if (joinerPokemon == null) { await joiner.SendMessageAsync("You have no Pokemon!"); return; }
        if (CreatorBattle == null) { await creator.SendMessageAsync("You have no Pokemon!"); return; }
        if (JoinerBattle == null) { await joiner.SendMessageAsync("You have no Pokemon!"); return; }

        // Status effects and if skip turn
        // Confusion
        bool NoCreatorTurn = await SkillHelper.ProcessConfusion(CreatorBattle, creator, joiner);
        bool NoJoinerTurn = await SkillHelper.ProcessConfusion(JoinerBattle, joiner, creator);

        // Check for Bide
        if (CreatorBattle.BideActive && CreatorBattle.BideTurns > 0)
        {
            CreatorBattle.BideTurns--;

            if (CreatorBattle.BideTurns == 0)
            {
                CreatorBattle.BideActive = false;
                JoinerBattle.Health -= CreatorBattle.BideDamage * 2; // Apply Bide damage to the opponent
                CreatorBattle.BideDamage = 0;
            }
            NoCreatorTurn = true;
        }

        if (JoinerBattle.BideActive && JoinerBattle.BideTurns > 0)
        {
            JoinerBattle.BideTurns--;

            if (JoinerBattle.BideTurns == 0)
            {
                JoinerBattle.BideActive = false;
                CreatorBattle.Health -= JoinerBattle.BideDamage * 2; // Apply Bide damage to the opponent
                JoinerBattle.BideDamage = 0;
            }
            NoJoinerTurn = true;
        }

        // Check for Bind
        if (CreatorBattle.BindActive && CreatorBattle.BindTurns > 0)
        {
            CreatorBattle.BindTurns--;
            CreatorBattle.Health -= CreatorBattle.BindDamage;

            if (CreatorBattle.BindTurns == 0)
            {
                CreatorBattle.BindActive = false;
                CreatorBattle.BindDamage = 0;
            }
            NoCreatorTurn = true;
        }

        if (JoinerBattle.BindActive && JoinerBattle.BindTurns > 0)
        {
            JoinerBattle.BindTurns--;
            JoinerBattle.Health -= JoinerBattle.BindDamage;

            if (JoinerBattle.BindTurns == 0)
            {
                JoinerBattle.BindActive = false;
                JoinerBattle.BindDamage = 0;
            }
            NoJoinerTurn = true;
        }

        // Dig
        if (CreatorBattle.Dig && NoCreatorTurn == false)
        {
            NoCreatorTurn = true;
            await SkillHelper.ProcessDig(JoinerBattle, CreatorBattle, creator, joiner);
        }

        if (JoinerBattle.Dig && NoJoinerTurn == false)
        {
            NoJoinerTurn = true;
            await SkillHelper.ProcessDig(CreatorBattle, JoinerBattle, joiner, creator);
        }

        // Skill
        string Creatorskill;
        string Joinerskill;

        // Ask for skill
        if (!NoCreatorTurn)
        {
            await creator.SendMessageAsync("Unfortunately, you cannot use your skill this turn. Please wait for your opponent to finish their turn.");
        }
        else
        {
            for (int i = 0; i < CreatorBattle.Skills.Count; i++)
            {
                var skill = CreatorBattle.Skills.ElementAt(i);
                await creator.SendMessageAsync($"{i + 1}. {skill.Name} - Power: {skill.BasePower} - PP: {skill.PowerPoints}/{skill.MaxPowerPoints}");
            }
            Creatorskill = await creator.GetInputAsync($"Please select a skill:");

        }

        if (!NoJoinerTurn)
        {
            await joiner.SendMessageAsync("Unfortunately, you cannot use your skill this turn. Please wait for your opponent to finish their turn.");
        }
        else
        {
            for (int i = 0; i < JoinerBattle.Skills.Count; i++)
            {
                var skill = JoinerBattle.Skills.ElementAt(i);
                await joiner.SendMessageAsync($"{i + 1}. {skill.Name} - Power: {skill.BasePower} - PP: {skill.PowerPoints}/{skill.MaxPowerPoints}");
            }
            Joinerskill = await joiner.GetInputAsync($"Please select a skill:");
        }

        // Decide who goes first
        if (CreatorBattle.Priority > JoinerBattle.Priority)
        {

        }
        else if (CreatorBattle.Priority < JoinerBattle.Priority)
        {

        }
        else
        {
            if (CreatorBattle.Speed > JoinerBattle.Speed)
            {

            }
            else if (CreatorBattle.Speed < JoinerBattle.Speed)
            {

            }
            else
            {
                // Randomize
                if (Random.Shared.NextDouble() <= 0.50)
                {

                }
                else
                {

                }
            }
        }




        turn++;
    }

    public async Task<bool?> StartBattle()
    {

        using (var context = new DatabaseContext())
        {
            creator = context.Users.FirstOrDefault(u => u.Username == creator!.Username);
            joiner = context.Users.FirstOrDefault(u => u.Username == joiner!.Username);

            creatorPokemon = context.PokemonMaster
                .Where(p => p.OwnerId == creator!.Id)
                .Where(p => p.Selected && !p.Starter)
                .ToList();

            joinerPokemon = context.PokemonMaster
                .Where(p => p.OwnerId == joiner!.Id)
                .Where(p => p.Selected && !p.Starter)
                .ToList();

            CreatorBattle = context.PokemonMaster
                .Where(p => p.OwnerId == creator!.Id)
                .FirstOrDefault(p => p.Starter);

            JoinerBattle = context.PokemonMaster
                .Where(p => p.OwnerId == joiner!.Id)
                .FirstOrDefault(p => p.Starter);
        }

        // Create stat backups
        _backupService.BackupPokemonStats(
            creatorPokemon,
            CreatorBattle!,
            joinerPokemon,
            JoinerBattle!);

        foreach (var pokemon in creatorPokemon!) { pokemon.ResetStats(); }
        foreach (var pokemon in joinerPokemon!) { pokemon.ResetStats(); }
        if (CreatorBattle != null) CreatorBattle.ResetStats();
        if (JoinerBattle != null) JoinerBattle.ResetStats();

        try
        {
            // Verify sessions are still valid before starting
            if (CreatorSession == null || JoinerSession == null ||
                !NetworkMethods.IsUsernameActive(creator!.Username!) ||
                !NetworkMethods.IsUsernameActive(joiner!.Username!))
            {
                Console.WriteLine("[Battle] One or both players disconnected before battle could start");
                return null;
            }

            // Check if players are still connected
            if (!NetworkMethods.IsUsernameActive(creator!.Username!) ||
                !NetworkMethods.IsUsernameActive(joiner!.Username!))
            {
                Console.WriteLine("[Battle] A player disconnected after welcome messages");
                return null;
            }

            do
            {
                await PrintMenu("creator");
                await PrintMenu("joiner");

                creatorResponse = false;
                joinerResponse = false;

                // Creator Choices
                var creatorTask = Task.Run(async () =>
                {
                    var response = await Choice(CreatorSession);
                    creatorResponse = true;
                    return response;
                });

                // Joiner Choices
                var joinerTask = Task.Run(async () =>
                {
                    var response = await JoinerSession.GetInputAsync("Please choose your action:");
                    joinerResponse = true;
                    return response;
                });

                // Thread Handling
                var timeoutTask = Task.Delay(60000);
                var playersTask = Task.WhenAll(creatorTask, joinerTask);

                var completedTask = await Task.WhenAny(timeoutTask, playersTask);

                if (completedTask == playersTask)
                {
                    // Both players provided input in time
                    string[] responses = await playersTask;
                    string creatorChoice = responses[0];
                    string joinerChoice = responses[1];

                    // Send confirmations
                    await JoinerSession.SendMessageAsync($"\n{creator.Username} chose: {creatorChoice}");
                    await CreatorSession.SendMessageAsync($"\n{joiner.Username} chose: {joinerChoice}");
                }
                else if (completedTask == timeoutTask)
                {
                    // If both players didnt respond in time
                    if (!creatorResponse && !joinerResponse)
                    {
                        await CreatorSession.SendMessageAsync("\nTime Limit reached. Battle Abandoned.");
                        await JoinerSession.SendMessageAsync("\nTime Limit reached. Battle Abandoned.");
                        return null;
                    }

                    // if only creator did not respond
                    if (!creatorResponse)
                    {
                        await CreatorSession.SendMessageAsync("\nTime limit reached. Battle Abandoned.");
                        await JoinerSession.SendMessageAsync($"\n{CreatorSession.Username} has reached his time limit.");
                        return false;
                    }

                    // if only joiner did not respond
                    if (!joinerResponse)
                    {
                        await JoinerSession.SendMessageAsync("\nTime limit reached. Battle Abandoned.");
                        await CreatorSession.SendMessageAsync($"\n{JoinerSession.Username} has reached his time limit.");
                        return true;
                    }
                }
                else
                {
                    Console.WriteLine("[Battle] Unexpected task completion");
                    return null;
                }

                turn++;
                return true;
                // Continue with battle logic...
            } while (true); // Add your battle termination condition here




            // Collect both inputs in parallel to avoid deadlocks
            // var creatorInputTask = Task.Run(async () =>
            // {
            //     try
            //     {
            //         await CreatorSession.SendMessageAsync("\nPlease type a message:");
            //         return await CreatorSession.GetInputAsync("") ?? "No input";
            //     }
            //     catch (Exception ex)
            //     {
            //         Console.WriteLine($"[Battle] Creator input error: {ex.Message}");
            //         return "Error";
            //     }
            // });

            // var joinerInputTask = Task.Run(async () =>
            // {
            //     try
            //     {
            //         await JoinerSession.SendMessageAsync("\nPlease type a message:");
            //         return await JoinerSession.GetInputAsync("") ?? "No input";
            //     }
            //     catch (Exception ex)
            //     {
            //         Console.WriteLine($"[Battle] Joiner input error: {ex.Message}");
            //         return "Error";
            //     }
            // });

            // // Wait for both tasks with a timeout
            // var allInputTasks = Task.WhenAll(creatorInputTask, joinerInputTask);
            // string creatorResponse;
            // string joinerResponse;
            // if (await Task.WhenAny(allInputTasks, Task.Delay(60000)) == allInputTasks)
            // {
            //     // Both inputs received
            //     string[] responses = await allInputTasks;
            //     creatorResponse = responses[0];
            //     joinerResponse = responses[1];

            //     Console.WriteLine($"[Battle] Creator said: {creatorResponse}");
            //     Console.WriteLine($"[Battle] Joiner said: {joinerResponse}");

            //     // Acknowledge inputs
            //     await CreatorSession.SendMessageAsync($"\nYou said: {creatorResponse}");
            //     await CreatorSession.SendMessageAsync($"\nYour opponent said: {joinerResponse}");
            //     await JoinerSession.SendMessageAsync($"\nYou said: {joinerResponse}");
            //     await JoinerSession.SendMessageAsync($"\nYour opponent said: {creatorResponse}");
            // }
            // else
            // {
            //     // Timeout occurred
            //     Console.WriteLine("[Battle] Input collection timed out");
            //     creatorResponse = "Timeout";
            //     joinerResponse = "Timeout";

            //     try
            //     {
            //         await CreatorSession.SendMessageAsync("\nTime limit reached. Continuing with battle.");
            //         await JoinerSession.SendMessageAsync("\nTime limit reached. Continuing with battle.");
            //     }
            //     catch (Exception ex)
            //     {
            //         Console.WriteLine($"[Battle] Error sending timeout messages: {ex.Message}");
            //     }
            // }


            // return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Battle] Error in StartBattle: {ex.Message}");
            return null;
        }
    }

    public void RestorePokemonStats()
    {

        _backupService.RestorePokemonStats(
            creatorPokemon!,
            CreatorBattle!,
            joinerPokemon!,
            JoinerBattle!,
            creatorFainted!,
            joinerFainted!);

        _backupService.ClearStatusConditions(
            creatorPokemon!,
            CreatorBattle!,
            joinerPokemon!,
            JoinerBattle!,
            creatorFainted!,
            joinerFainted!);

        if (creatorPokemon!.Count > 0 && creatorPokemon != null)
            foreach (var pokemon in creatorPokemon!) { pokemon.ResetStats(); }

        if (joinerPokemon!.Count > 0 && joinerPokemon != null)
            foreach (var pokemon in joinerPokemon!) { pokemon.ResetStats(); }

        if (CreatorBattle != null)
            CreatorBattle.ResetStats();

        if (JoinerBattle != null)
            JoinerBattle.ResetStats();

        if (creatorFainted!.Count > 0 && creatorFainted != null)
            foreach (var pokemon in creatorFainted) { pokemon.ResetStats(); }

        if (joinerFainted!.Count > 0 && joinerFainted != null)
            foreach (var pokemon in joinerFainted) { pokemon.ResetStats(); }
    }

    public bool? CheckWinner()
    {
        if (creatorFainted != null && creatorFainted.Count == 6)
        {
            // Creator has no Pokemon left
            return true;
        }
        else if (joinerFainted != null && joinerFainted.Count == 6)
        {
            // Joiner has no Pokemon left
            return false;
        }
        return null;
    }

    public async Task FaintSwitchPokemon(ClientSession switcher, string joinerOrCreator)
    {
        string pokemonName;
        var pokemonList = new List<string>();

        // List of Names of Joiner Pokemon 
        if (joinerOrCreator == "joiner")
        {
            pokemonList = joinerPokemon!.Select(p => p.Name).ToList()!;
        }
        else if (joinerOrCreator == "creator")
        {
            pokemonList = creatorPokemon!.Select(p => p.Name).ToList()!;
        }

        if (joinerOrCreator == "joiner")
        {
            do
            {
                pokemonName = await switcher.GetInputAsync("Please enter the name of the Pokemon you want to switch to:");
            } while (string.IsNullOrEmpty(pokemonName) || !pokemonList.Contains(pokemonName));

            var pokemon = joinerPokemon!.FirstOrDefault(p => p.Name! == pokemonName);
            joinerPokemon!.Remove(pokemon!);

            // Swap
            joinerFainted!.Add(JoinerBattle!);
            JoinerBattle = pokemon;
        }
        else if (joinerOrCreator == "creator")
        {
            do
            {
                pokemonName = await switcher.GetInputAsync("Please enter the name of the Pokemon you want to switch to:");
            } while (string.IsNullOrEmpty(pokemonName) || !pokemonList.Contains(pokemonName));

            var pokemon = creatorPokemon!.FirstOrDefault(p => p.Name! == pokemonName);
            creatorPokemon!.Remove(pokemon!);

            // Swap
            creatorFainted!.Add(CreatorBattle!);
            CreatorBattle = pokemon;
        }
    }

    public async Task PrintMenu(string sender)
    {
        int creatorHPBarLength = 20;
        int joinerHPBarLength = 20;
        double creatorHPPercentage = (double)CreatorBattle!.Health / CreatorBattle.MaxHealth;
        double joinerHPPercentage = (double)JoinerBattle!.Health / JoinerBattle.MaxHealth;
        int creatorFilledBars = (int)(creatorHPPercentage * creatorHPBarLength);
        int joinerFilledBars = (int)(joinerHPPercentage * joinerHPBarLength);

        string creatorHPBar = $"[{new string('█', creatorFilledBars)}{new string('░', creatorHPBarLength - creatorFilledBars)}]";
        string joinerHPBar = $"[{new string('█', joinerFilledBars)}{new string('░', joinerHPBarLength - joinerFilledBars)}]";

        // Format types
        string creatorTypes = CreatorBattle.Type!;
        string joinerTypes = JoinerBattle.Type!;

        string creatorHealth = $"{CreatorBattle.Health}/{CreatorBattle.MaxHealth}";
        string joinerHealth = $"{JoinerBattle.Health}/{JoinerBattle.MaxHealth}";

        string blank = "";

        string creatorAttackDefense = $"{blank,13}ATK: {CreatorBattle.Attack,-16}DEF: {CreatorBattle.Defense,-12}";
        string creatorSpASpD = $"{blank,13}SpA: {CreatorBattle.SpecialAttack,-16}SpD: {CreatorBattle.SpecialDefense,-16}";
        string creatorSpeed = $"SPD: {CreatorBattle.Speed,-4}";

        string joinerAttackDefense = $"{blank,-13}ATK: {JoinerBattle.Attack,-16}DEF: {JoinerBattle.Defense,-12}";
        string joinerSpASpD = $"{blank,-13}SpA: {JoinerBattle.SpecialAttack,-16}SpD: {JoinerBattle.SpecialDefense,-12}";
        string joinerSpeed = $"SPD: {JoinerBattle.Speed,-4}";

        // Count Pokemon
        string creatorPokemonCount = $"{creator!.Username}'s Pokémon: {creatorPokemon!.Count}";
        string joinerPokemonCount = $"{joiner!.Username}'s Pokémon: {joinerPokemon!.Count}";

        StringBuilder sb = new StringBuilder();
        sb.Append(@$"
╔═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
║{ExtraMethods.CenterAlign($"POKÉMON BATTLE {turn}", 115)}║
║{ExtraMethods.CenterAlign($"{creator.Username} VS {joiner.Username}", 115)}║
╠═════════════════════════════════════════════════════════╦═════════════════════════════════════════════════════════╣
║{ExtraMethods.CenterAlign($"{creator.Username}'s POKÉMON", 57)}║{ExtraMethods.CenterAlign($"{joiner.Username}'s POKÉMON", 57)}║
╠═════════════════════════════════════════════════════════╬═════════════════════════════════════════════════════════╣
║                                                         ║                                                         ║
║{ExtraMethods.CenterAlign($"{CreatorBattle.Name}", 57)}║{ExtraMethods.CenterAlign($"{JoinerBattle.Name}", 57)}║
║    Type: {creatorTypes,-47}║    Type: {joinerTypes,-47}║
║    HP: {creatorHealth,-49}║    HP: {joinerHealth,-49}║
║    {creatorHPBar,-53}║    {joinerHPBar,-53}║
║                                                         ║                                                         ║
║{creatorAttackDefense,-57}║{joinerAttackDefense,-57}║
║{creatorSpASpD,-57}║{joinerSpASpD,-57}║
║{ExtraMethods.CenterAlign($"{creatorSpeed}", 57)}║{ExtraMethods.CenterAlign($"{joinerSpeed}", 57)}║
║                                                         ║                                                         ║");

        string creatorStatus = "None";

        if (CreatorBattle.Paralyzed
        || CreatorBattle.Freezing
        || CreatorBattle.Burning
        || CreatorBattle.Poisoned
        || CreatorBattle.BadlyPoisoned
        || CreatorBattle.Sleeping)
        {
            creatorStatus = "";
        }
        string CreatorStatusString = $"Status: {creatorStatus}";

        string joinerStatus = "None";

        if (JoinerBattle.Paralyzed
        || JoinerBattle.Freezing
        || JoinerBattle.Burning
        || JoinerBattle.Poisoned
        || JoinerBattle.BadlyPoisoned
        || JoinerBattle.Sleeping)
        {
            joinerStatus = "";
        }
        string JoinerStatusString = $"Status: {joinerStatus}";

        sb.Append($"\n║{ExtraMethods.CenterAlign($"{CreatorStatusString.Trim()}", 57)}║{ExtraMethods.CenterAlign($"{JoinerStatusString.Trim()}", 57)}║");

        // Total Status effects count
        if (joinerStatus != "None" && creatorStatus != "None")
        {
            int creatorStatusCount = 0;
            int joinerStatusCount = 0;
            int StatusRows;

            var CreatorStatusList = new List<string>();
            var JoinerStatusList = new List<string>();

            // Creator Status
            if (CreatorBattle.Paralyzed)
            {
                creatorStatusCount++;
                CreatorStatusList.Add("Paralyzed");
            }

            if (CreatorBattle.Freezing)
            {
                creatorStatusCount++;
                CreatorStatusList.Add("Freezing");
            }

            if (CreatorBattle.Burning)
            {
                creatorStatusCount++;
                CreatorStatusList.Add("Burning");
            }

            if (CreatorBattle.Poisoned)
            {
                creatorStatusCount++;
                CreatorStatusList.Add("Poisoned");
            }

            if (CreatorBattle.BadlyPoisoned)
            {
                creatorStatusCount++;
                CreatorStatusList.Add("Badly Poisoned");
            }

            if (CreatorBattle.Sleeping)
            {
                creatorStatusCount++;
                CreatorStatusList.Add("Sleeping");
            }

            // Joiner Status
            if (JoinerBattle.Paralyzed)
            {
                joinerStatusCount++;
                JoinerStatusList.Add("Paralyzed");
            }

            if (JoinerBattle.Freezing)
            {
                joinerStatusCount++;
                JoinerStatusList.Add("Freezing");
            }

            if (JoinerBattle.Burning)
            {
                joinerStatusCount++;
                JoinerStatusList.Add("Burning");
            }

            if (JoinerBattle.Poisoned)
            {
                joinerStatusCount++;
                JoinerStatusList.Add("Poisoned");
            }

            if (JoinerBattle.BadlyPoisoned)
            {
                joinerStatusCount++;
                JoinerStatusList.Add("Badly Poisoned");
            }

            if (JoinerBattle.Sleeping)
            {
                joinerStatusCount++;
                JoinerStatusList.Add("Sleeping");
            }

            // Print out the status effects
            if (creatorStatusCount > joinerStatusCount)
            {
                StatusRows = creatorStatusCount;
            }
            else
            {
                StatusRows = joinerStatusCount;
            }

            for (int i = 0; i < StatusRows; i++)
            {
                string creatorStatusEffect = i < CreatorStatusList.Count ? CreatorStatusList[i] : "";
                string joinerStatusEffect = i < JoinerStatusList.Count ? JoinerStatusList[i] : "";

                sb.Append($"\n║     - {creatorStatusEffect,50}║     - {joinerStatusEffect,50}║");
            }
        }


        sb.Append(@$"
╠═════════════════════════════════════════════════════════╩═════════════════════════════════════════════════════════╣
║{ExtraMethods.CenterAlign($"{creatorPokemonCount}", 57)} {ExtraMethods.CenterAlign($"{joinerPokemonCount}", 57)}║
╚═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝
");

        if (sender == "creator")
        {
            await CreatorSession.SendMessageAsync(sb.ToString());
        }
        else if (sender == "joiner")
        {
            await JoinerSession.SendMessageAsync(sb.ToString());
        }

    }

    public async Task<string> Choice(ClientSession session)
    {
        while (true)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@$"
═════════════════ Choice ═════════════════
 [1] Attack
 [2] Switch Pokémon
 [RESIGN] Resign
");
            await session.SendMessageAsync(sb.ToString());

            string choice = await session.GetInputAsync("\nChoice:");

            switch (choice.ToUpper())
            {
                case "1":

                    return "Attack";
                case "2":
                    return await ChoiceSwitch(session);
                case "RESIGN":
                    return "Resign";
                default:
                    await session.SendMessageAsync("Invalid choice. Please try again.");
                    continue;
            }
        }

    }

    public async Task<string> ChoiceSwitch(ClientSession session)
    {
        while (true)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("════════════ POKÉMON AVAILABLE ════════════");

            int i = 1;
            var pokemonCollection = CreatorSession == session ? creatorPokemon : joinerPokemon;
            var pokedict = new Dictionary<int, string>();

            var BattlePokemon = CreatorSession == session ? CreatorBattle : JoinerBattle;

            if (pokemonCollection == null || pokemonCollection.Count == 0)
            {
                await session.SendMessageAsync("You have no Pokémon available to switch to.");
                return await Choice(session);
            }

            foreach (var pokemon in pokemonCollection)
            {
                sb.Append($"\n [{i}] {pokemon.Name} - HP: {pokemon.Health}/{pokemon.MaxHealth}");
                pokedict.Add(i, pokemon.Name!);
                i++;
            }

            sb.Append(@$" [B] Back");

            await session.SendMessageAsync(sb.ToString());

            string choice = await session.GetInputAsync("\nChoice:");

            if (int.TryParse(choice, out int choiceNumber) && choiceNumber >= 1 && choiceNumber <= pokemonCollection.Count)
            {
                var selectedPokemon = pokemonCollection
                    .FirstOrDefault(p => p.Name == pokedict[choiceNumber]);

                if (CreatorSession == session)
                {
                    creatorPokemon!.Add(CreatorBattle!);
                    CreatorBattle = selectedPokemon;
                    creatorPokemon!.Remove(selectedPokemon!);
                }
                else
                {
                    joinerPokemon!.Add(JoinerBattle!);
                    JoinerBattle = selectedPokemon;
                    joinerPokemon!.Remove(selectedPokemon!);
                }
                return $"Switch | {selectedPokemon!.Name}";
            }
            else if (choice.ToUpper() == "B")
            {
                return await Choice(session);
            }
            else
            {
                await session.SendMessageAsync("Invalid choice. Please try again.");
                continue;
            }
        }
    }

    public async Task<string> ChoiceAttack(ClientSession session)
    {
        while (true)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("═════════════════ ATTACK ═════════════════");

            int i = 1;
            var BattlePokemon = CreatorSession == session ? CreatorBattle : JoinerBattle;

            var BattleSkills = BattlePokemon!.Skills;

            foreach (var skill in BattleSkills)
            {
                sb.Append($"\n [{i}] {skill.Name} - Power: {skill.BasePower} - PP: {skill.PowerPoints}/{skill.MaxPowerPoints}");
                i++;
            }

            sb.Append(@$" [B] Back");

            await session.SendMessageAsync(sb.ToString());

            string choice = await session.GetInputAsync("\nChoice:");

            if (int.TryParse(choice, out int choiceNumber) && choiceNumber >= 1 && choiceNumber <= BattleSkills.Count)
            {
                var selectedSkill = BattleSkills.ElementAt(choiceNumber - 1);
                return $"Attack | {selectedSkill.Name}";
            }
            else if (choice.ToUpper() == "B")
            {
                return await Choice(session);
            }
            else
            {
                await session.SendMessageAsync("Invalid choice. Please try again.");
                continue;
            }
        }
        
    }
    
}