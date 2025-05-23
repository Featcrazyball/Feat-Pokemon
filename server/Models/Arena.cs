using Server;
using PokemonPocket;
using Models;
using System.Text;
using Database;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data.SQLite;

namespace Arena;

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
    public int turn { get; set; } = 1;

    // Response
    public bool creatorResponse { get; set; } = false;
    public bool joinerResponse { get; set; } = false;

    // Winner
    public bool? GameWinner { get; set; } = null;

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
    public async Task<bool?> CheckStats()
    {
        var creator = CreatorSession;
        var joiner = JoinerSession;
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

            var CreatorFaintedTask = Task.Run(async () =>
            {
                await creator.SendMessageAsync($"Your {CreatorBattle.Name} has fainted!");
            });

            var JoinerFaintedTask = Task.Run(async () =>
            {
                await joiner.SendMessageAsync($"{creator.Username} {CreatorBattle.Name} has fainted!");
            });
            await Task.WhenAll(CreatorFaintedTask, JoinerFaintedTask);

            // print out all remaining pokemon
            if (creatorPokemon.Count > 0)
            {
                var creatorTask = Task.Run(async () =>
                {
                    await FaintSwitchPokemon(creator);
                });

                var joinerTask = Task.Run(async () =>
                {
                    await joiner.SendMessageAsync("Please wait for your opponent to choose their next Pokémon.");
                });

                var tasks = Task.WhenAll(creatorTask, joinerTask);
                await Task.WhenAll(tasks);
                await creator.SendMessageAsync($"You have switched to {CreatorBattle.Name}.");
                await joiner.SendMessageAsync($"{creator.Username} has switched to {CreatorBattle.Name}.");
            }
            else
            {
                return false;
            }
        }

        if (JoinerBattle.Health <= 0)
        {
            var CreatorFaintedTask = Task.Run(async () =>
            {
                await creator.SendMessageAsync($"{joiner.Username}'s {JoinerBattle.Name} has fainted!");
            });

            var JoinerFaintedTask = Task.Run(async () =>
            {
                await joiner.SendMessageAsync($"Your {JoinerBattle.Name} has fainted!");
            });
            await Task.WhenAll(JoinerFaintedTask, CreatorFaintedTask);

            // print out all remaining pokemon
            if (joinerPokemon.Count > 0)
            {
                var joinerTask = Task.Run(async () =>
                {
                    await FaintSwitchPokemon(joiner);
                });

                var creatorTask = Task.Run(async () =>
                {
                    await creator.SendMessageAsync("Please wait for your opponent to choose their next Pokémon.");
                });

                var tasks = Task.WhenAll(joinerTask, creatorTask);
                await Task.WhenAll(tasks);

                await joiner.SendMessageAsync($"You have switched to {JoinerBattle.Name}.");
                await creator.SendMessageAsync($"{joiner.Username} has switched to {JoinerBattle.Name}.");
            }
            else
            {
                return true;
            }
        }

        return null;
    }

    public async Task<bool?> StartBattle()
    {

        using (var context = new DatabaseContext())
        {
            creator = context.Users.FirstOrDefault(u => u.Username == creator!.Username);
            joiner = context.Users.FirstOrDefault(u => u.Username == joiner!.Username);

            creatorPokemon = context.PokemonMaster
                .Include(p => p.Skills)  // <-- Include skills
                .Where(p => p.OwnerId == creator!.Id)
                .Where(p => p.Selected && !p.Starter)
                .ToList();

            joinerPokemon = context.PokemonMaster
                .Include(p => p.Skills)  // <-- Include skills 
                .Where(p => p.OwnerId == joiner!.Id)
                .Where(p => p.Selected && !p.Starter)
                .ToList();

            CreatorBattle = context.PokemonMaster
                .Include(p => p.Skills)  // <-- Include skills
                .Where(p => p.OwnerId == creator!.Id)
                .FirstOrDefault(p => p.Starter);

            JoinerBattle = context.PokemonMaster
                .Include(p => p.Skills)  // <-- Include skills
                .Where(p => p.OwnerId == joiner!.Id)
                .FirstOrDefault(p => p.Starter);

            // Reset Powerpoints of all skills
            foreach (var pokemon in creatorPokemon!)
            {
                foreach (var skill in pokemon.Skills)
                {
                    skill.ResetPowerPoints();
                }
            }

            foreach (var pokemon in joinerPokemon!)
            {
                foreach (var skill in pokemon.Skills)
                {
                    skill.ResetPowerPoints();
                }
            }

            // Reset Powerpoints of all skills
            if (CreatorBattle != null)
            {
                foreach (var skill in CreatorBattle.Skills)
                {
                    skill.ResetPowerPoints();
                }
            }

            if (JoinerBattle != null)
            {
                foreach (var skill in JoinerBattle.Skills)
                {
                    skill.ResetPowerPoints();
                }
            }
        }

        // Create stat backups
        _backupService.BackupPokemonStats(
            creatorPokemon,
            CreatorBattle!,
            joinerPokemon,
            JoinerBattle!);

        foreach (var pokemon in creatorPokemon!) { pokemon.ResetStats(); pokemon.PayDay = 100; }
        foreach (var pokemon in joinerPokemon!) { pokemon.ResetStats(); pokemon.PayDay = 100; }
        if (CreatorBattle != null)
        {
            CreatorBattle.ResetStats();
            CreatorBattle.PayDay = 100;
        }
        
        if (JoinerBattle != null)
        {
            JoinerBattle.ResetStats();
            JoinerBattle.PayDay = 100;
        }

        bool? winner = null;
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
                creatorResponse = false;
                joinerResponse = false;
                
                // Creator Choices
                var creatorTask = Task.Run(async () =>
                {
                    await PrintMenu("creator");
                    var response = await Choice(CreatorSession);
                    creatorResponse = true;
                    await CreatorSession.SendMessageAsync("\nPlease wait for your opponent to choose their next move.");
                    return response;
                });

                // Joiner Choices
                var joinerTask = Task.Run(async () =>
                {
                    await PrintMenu("joiner");
                    var response = await Choice(JoinerSession);
                    joinerResponse = true;
                    await JoinerSession.SendMessageAsync("\nPlease wait for your opponent to choose their next move.");
                    return response;
                });

                var playersTask = Task.WhenAll(creatorTask, joinerTask);

                // Both players provided input in time
                string[] responses = await playersTask;
                string creatorChoice = responses[0];
                string joinerChoice = responses[1];

                // Process the choices
                string CreatorAction = creatorChoice.Split('|')[0].Trim();
                string JoinerAction = joinerChoice.Split('|')[0].Trim();

                string CreatorActionFollowUp = creatorChoice.Split('|')[1].Trim();
                string JoinerActionFollowUp = joinerChoice.Split('|')[1].Trim();

                // if resign is used, end the battle
                if (creatorChoice.ToLower() == "resign" || joinerChoice.ToLower() == "resign")
                {
                    if (creatorChoice == "resign")
                    {
                        await CreatorSession.SendMessageAsync("\nYou have resigned from the battle.");
                        await JoinerSession.SendMessageAsync($"\n{CreatorSession.Username} has resigned from the battle.");
                        return false;
                    }
                    else
                    {
                        await JoinerSession.SendMessageAsync("\nYou have resigned from the battle.");
                        await CreatorSession.SendMessageAsync($"\n{JoinerSession.Username} has resigned from the battle.");
                        return true;
                    }
                }

                // Calculate Speed
                string FirstTurn = CalculateSpeed(CreatorAction, JoinerAction, CreatorActionFollowUp, JoinerActionFollowUp);

                ClientSession FirstTurnSession;
                ClientSession SecondTurnSession;
                string FirstChoice;
                string SecondChoice;

                if (FirstTurn == "creator")
                {
                    FirstTurnSession = CreatorSession;
                    SecondTurnSession = JoinerSession;

                    FirstChoice = creatorChoice;
                    SecondChoice = joinerChoice;
                }
                else
                {
                    FirstTurnSession = JoinerSession;
                    SecondTurnSession = CreatorSession;

                    FirstChoice = joinerChoice;
                    SecondChoice = creatorChoice;
                }

                Console.WriteLine($"[Battle] {FirstTurnSession.Username} will go first with {FirstChoice} and {SecondTurnSession.Username} will go second with {SecondChoice}");
                Console.WriteLine($"[Battle] Creator: {CreatorAction} | Joiner: {JoinerAction}");

                // Turn
                bool? battleResult = await AdministerBattle(FirstTurnSession, SecondTurnSession, FirstChoice, SecondChoice);
                if (battleResult != null)
                {
                    winner = battleResult;
                    GameWinner = battleResult;
                }
                else
                {
                    winner = CheckWinner();
                }

                turn++;
            } while (GameWinner == null);

            Console.WriteLine($"[Battle] Winner: {winner}");
            RestorePokemonStats();
            RemoveTempSkills();

            if (GameWinner == true)
            {
                await CreatorSession.SendMessageAsync("\nYou have won the battle!");
                await JoinerSession.SendMessageAsync("\nYou have lost the battle!");
                return true;
            }
            else if (GameWinner == false)
            {
                await CreatorSession.SendMessageAsync("\nYou have lost the battle!");
                await JoinerSession.SendMessageAsync("\nYou have won the battle!");
                return false;
            }
            else if (GameWinner == null)
            {
                await CreatorSession.SendMessageAsync("\nBattle has been abandoned");
                await JoinerSession.SendMessageAsync("\nBattle has been abandoned.");
            }

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Battle] Error in StartBattle: {ex.Message}");
            return null;
        }
    }

    public void RemoveTempSkills()
    {
        // Remove metronome, mimic and transform skills
        foreach (var pokemon in creatorFainted!)
        {
            var tempskills = pokemon.Skills.Where(s => s.Metronome || s.Mimic || s.Transform).ToList();
            foreach (var skill in tempskills)
            {
                pokemon.Skills.Remove(skill);
            }
            pokemon.Mimic = false;
            pokemon.Transform = false;
            pokemon.Metronome = false;
        }

        foreach (var pokemon in joinerFainted!)
        {
            var tempskills = pokemon.Skills.Where(s => s.Metronome || s.Mimic || s.Transform).ToList();
            foreach (var skill in tempskills)
            {
                pokemon.Skills.Remove(skill);
            }
            pokemon.Mimic = false;
            pokemon.Transform = false;
            pokemon.Metronome = false;
        }

        // Remove metronome, mimic and transform skills
        if (CreatorBattle != null)
        {
            var tempskills = CreatorBattle.Skills.Where(s => s.Metronome || s.Mimic || s.Transform).ToList();
            foreach (var skill in tempskills)
            {
                CreatorBattle.Skills.Remove(skill);
            }
            CreatorBattle.Mimic = false;
            CreatorBattle.Transform = false;
            CreatorBattle.Metronome = false;
        }

        if (JoinerBattle != null)
        {
            var tempskills = JoinerBattle.Skills.Where(s => s.Metronome || s.Mimic || s.Transform).ToList();
            foreach (var skill in tempskills)
            {
                JoinerBattle.Skills.Remove(skill);
            }
            JoinerBattle.Mimic = false;
            JoinerBattle.Transform = false;
            JoinerBattle.Metronome = false;
        }

        // Original List
        if (creatorPokemon != null)
        {
            foreach (var pokemon in creatorPokemon)
            {
                var tempskills = pokemon.Skills.Where(s => s.Metronome || s.Mimic || s.Transform).ToList();
                foreach (var skill in tempskills)
                {
                    pokemon.Skills.Remove(skill);
                }
                pokemon.Mimic = false;
                pokemon.Metronome = false;
                pokemon.Transform = false;
            }
        }

        if (joinerPokemon != null)
        {
            foreach (var pokemon in joinerPokemon)
            {
                var tempskills = pokemon.Skills.Where(s => s.Metronome || s.Mimic || s.Transform).ToList();
                foreach (var skill in tempskills)
                {
                    pokemon.Skills.Remove(skill);
                }
                pokemon.Mimic = false;
                pokemon.Transform = false;
                pokemon.Metronome = false;
            }
        }

        Console.WriteLine("[Battle] Removed temporary skills from all Pokemon.");
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

        Console.WriteLine("[Battle] Restored stats for all Pokemon.");
    }

    public bool? CheckWinner()
    {
        if (creatorFainted != null && creatorFainted.Count == 6)
        {
            // Creator has no Pokemon left
            GameWinner = false;
            return false;
        }
        else if (joinerFainted != null && joinerFainted.Count == 6)
        {
            // Joiner has no Pokemon left
            GameWinner = true;
            return true;
        }
        return null;
    }

    public async Task FaintSwitchPokemon(ClientSession switcher)
    {
        if (switcher == JoinerSession)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("\n-------------------------------------------------------");
            sb.AppendLine($"\nYour {JoinerBattle!.Name} has fainted!");
            sb.AppendLine("\nPlease choose a Pokémon to switch to:");

            int i = 1;

            if (joinerPokemon == null || joinerPokemon.Count == 0)
            {
                await switcher.SendMessageAsync("\nYou have no Pokémon left to switch to.");
                GameWinner = true;
                return;
            }

            foreach (var poke in joinerPokemon!)
            {
                sb.Append($"\n [{i}] {poke.Name} - HP: {poke.Health}/{poke.MaxHealth}");
                i++;
            }

            await switcher.SendMessageAsync(sb.ToString());

            string pokemonName;
            int choice;
            do
            {
                pokemonName = await switcher.GetInputAsync("\nChoice::");
            } while (!int.TryParse(pokemonName, out choice) || choice < 1 || choice > joinerPokemon!.Count);

            var pokemon = joinerPokemon!.ElementAt(choice - 1);
            joinerPokemon!.Remove(pokemon!);

            // Swap
            joinerFainted!.Add(JoinerBattle!);
            JoinerBattle = pokemon;
        }
        else if (switcher == CreatorSession)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("\n-------------------------------------------------------");
            sb.AppendLine($"\nYour {CreatorBattle!.Name} has fainted!");
            sb.AppendLine("\nPlease choose a Pokémon to switch to:");
            int i = 1;

            if (creatorPokemon == null || creatorPokemon.Count == 0)
            {
                await switcher.SendMessageAsync("\nYou have no Pokémon left to switch to.");
                GameWinner = false;
                return;
            }

            foreach (var poke in creatorPokemon!)
            {
                sb.Append($"\n [{i}] {poke.Name} - HP: {poke.Health}/{poke.MaxHealth}");
                i++;
            }
            await switcher.SendMessageAsync(sb.ToString());

            string pokemonName;
            int choice;
            do
            {
                pokemonName = await switcher.GetInputAsync("\nChoice::");
            } while (!int.TryParse(pokemonName, out choice) || choice < 1 || choice > creatorPokemon!.Count);

            var pokemon = creatorPokemon!.ElementAt(choice - 1);
            creatorPokemon!.Remove(pokemon!);

            // Swap
            creatorFainted!.Add(CreatorBattle!);
            CreatorBattle = pokemon;
        }
    }

    public string CalculateSpeed(string creatorChoice, string joinerChoice, string creatorAction, string joinerAction)
    {
        int creatorPriority = 0;
        int joinerPriority = 0;

        float creatorSpeed = CreatorBattle!.Speed;
        float joinerSpeed = JoinerBattle!.Speed;

        if (creatorChoice.Contains("Attack"))
        {
            var skill = CreatorBattle.Skills.FirstOrDefault(s => s.Name == creatorAction);
            if (skill != null)
            {
                creatorPriority = skill.Priority;
            }
        }

        if (joinerChoice.Contains("Attack"))
        {
            var skill = JoinerBattle.Skills.FirstOrDefault(s => s.Name == joinerAction);
            if (skill != null)
            {
                joinerPriority = skill.Priority;
            }
        }

        if (creatorPriority > joinerPriority)
        {
            return "creator";
        }
        else if (creatorPriority < joinerPriority)
        {
            return "joiner";
        }
        else
        {
            if (creatorSpeed > joinerSpeed)
            {
                return "creator";
            }
            else if (creatorSpeed < joinerSpeed)
            {
                return "joiner";
            }
            else
            {
                // Randomize
                if (Random.Shared.NextDouble() <= 0.50)
                {
                    return "creator";
                }
                else
                {
                    return "joiner";
                }
            }
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
║{ExtraMethods.CenterAlign($"POKÉMON BATTLE TURN {turn}", 115)}║
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
        bool creatorHasStatus = false;

        if (CreatorBattle.Paralyzed
        || CreatorBattle.Freezing
        || CreatorBattle.Burning
        || CreatorBattle.Poisoned
        || CreatorBattle.BadlyPoisoned
        || CreatorBattle.Sleeping)
        {
            creatorStatus = "";
            creatorHasStatus = true;
        }
        string CreatorStatusString = $"    Status: {creatorStatus}";

        string joinerStatus = "None";
        bool joinerHasStatus = false;

        if (JoinerBattle.Paralyzed
        || JoinerBattle.Freezing
        || JoinerBattle.Burning
        || JoinerBattle.Poisoned
        || JoinerBattle.BadlyPoisoned
        || JoinerBattle.Sleeping)
        {
            joinerStatus = "";
            joinerHasStatus = true;
        }
        string JoinerStatusString = $"    Status: {joinerStatus}";

        sb.Append($"\n║{$"{CreatorStatusString}", -57}║{$"{JoinerStatusString}", -57}║");

        // Total Status effects count
        if (joinerHasStatus || creatorHasStatus)
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
                string creatorStatusEffect = i < CreatorStatusList.Count ? $"- {CreatorStatusList[i]}" : "";
                string joinerStatusEffect = i < JoinerStatusList.Count ? $"- {JoinerStatusList[i]}" : "";

                sb.Append($"\n║     {creatorStatusEffect,-52}║     {joinerStatusEffect,-52}║");
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
        // Hyper Beam Recharge Check
        if (session == CreatorSession && CreatorBattle!.HyperBeamRecharge)
        {
            await session.SendMessageAsync("Due to hyperbeam, you must recharge this turn.");
            return "Attack | Hyper Beam";
        }
        else if (session == JoinerSession && JoinerBattle!.HyperBeamRecharge)
        {
            await session.SendMessageAsync("You must recharge after using Hyper Beam.");
            return "Attack | Hyper Beam";
        }

        // Check for petal dance
        if (session == CreatorSession && CreatorBattle!.PetalDance)
        {
            await session.SendMessageAsync("Petal Dance will be used.");
            return "Attack | Petal Dance";
        }
        else if (session == JoinerSession && JoinerBattle!.PetalDance)
        {
            await session.SendMessageAsync("Petal Dance will be used.");
            return "Attack | Petal Dance";
        }

        // Check for thrash
        if (session == CreatorSession && CreatorBattle!.Thrashing)
        {
            await session.SendMessageAsync("Thrash will be used.");
            return "Attack | Thrash";
        }
        else if (session == JoinerSession && JoinerBattle!.Thrashing)
        {
            await session.SendMessageAsync("Thrash will be used.");
            return "Attack | Thrash";
        }


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

            if (session == CreatorSession && choice == "2")
            {
                if (CreatorBattle!.BindActive)
                {
                    await session.SendMessageAsync("You cannot switch Pokémon while Bind is active.");
                    continue;
                }
            }
            else if (session == JoinerSession && choice == "2")
            {
                if (JoinerBattle!.BindActive)
                {
                    await session.SendMessageAsync("You cannot switch Pokémon while Bind is active.");
                    continue;
                }
            }

            switch (choice.ToUpper())
            {
                case "1":
                    Console.WriteLine($"Attack option selected by {session.Username}");
                    var response = await ChoiceAttack(session);
                    Console.WriteLine($"{response} option selected by {session.Username}");
                    return response;
                case "2":
                    Console.WriteLine($"Attack option selected by {session.Username}");
                    var responseAttack = await ChoiceSwitch(session);
                    Console.WriteLine($"{responseAttack} option selected by {session.Username}");
                    return responseAttack;
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

            sb.AppendLine("\n [B] Back");

            await session.SendMessageAsync(sb.ToString());

            string choice = await session.GetInputAsync("\nChoice:");

            if (int.TryParse(choice, out int choiceNumber) && choiceNumber >= 1 && choiceNumber <= pokemonCollection.Count)
            {
                var selectedPokemon = pokemonCollection
                    .FirstOrDefault(p => p.Name == pokedict[choiceNumber]);

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
            var BattlePokemon = session == CreatorSession ? CreatorBattle : JoinerBattle;
            
            var BattleSkills = BattlePokemon!.Transform 
                ? BattlePokemon.Skills.Where(s => s.Transform).ToList()
                : BattlePokemon.Skills.Where(s => !s.Transform && !s.Metronome && !s.Mimic).ToList();

            if (BattlePokemon.Transform)
            {
                BattleSkills = BattlePokemon.Skills
                    .Where(s => s.Transform)
                    .ToList();
            }

            foreach (var skill in BattleSkills)
            {
                sb.AppendLine($"\n [{i}] {skill.Name} - Power: {skill.BasePower} - PP: {skill.PowerPoints}/{skill.MaxPowerPoints}");
                i++;
            }

            sb.AppendLine("\n [P] Pass");
            sb.AppendLine("\n [B] Back");

            await session.SendMessageAsync(sb.ToString());

            string choice = await session.GetInputAsync("\nChoice:");

            if (choice.ToUpper() == "P")
            {
                return "Pass | Pass";
            }
            else if (choice.ToUpper() == "B")
            {
                return await Choice(session);
            }

            if (choice == "Mimic" && session == CreatorSession && CreatorBattle!.Mimic)
            {
                await session.SendMessageAsync("Mimic can only be used once.");
                continue;
            }
            else if (choice == "Mimic" && session == JoinerSession && JoinerBattle!.Mimic)
            {
                await session.SendMessageAsync("Mimic can only be used once.");
                continue;
            }

            if (int.TryParse(choice, out int choiceNumber) && choiceNumber >= 1 && choiceNumber <= BattleSkills.Count)
            {
                var selectedSkill = BattleSkills.ElementAt(choiceNumber - 1);

                //  Check for PP
                if (selectedSkill.PowerPoints <= 0)
                {
                    await session.SendMessageAsync($"You cannot use {selectedSkill.Name} because it has no PP left.");
                    continue;
                }

                // Check for Bide
                if (session == CreatorSession)
                {
                    if (CreatorBattle!.BideActive && selectedSkill.Name == "Bide")
                    {
                        await session.SendMessageAsync("You cannot use Bide while it is active.");
                        continue;
                    }
                }
                else if (session == JoinerSession)
                {
                    if (JoinerBattle!.BideActive && selectedSkill.Name == "Bide")
                    {
                        await session.SendMessageAsync("You cannot use Bide while it is active.");
                        continue;
                    }
                }

                // Check for Disabled
                if (session == CreatorSession)
                {
                    if (CreatorBattle!.Disable && CreatorBattle.DisabledSkill == selectedSkill.Name)
                    {
                        await session.SendMessageAsync($"You cannot use {CreatorBattle.DisabledSkill} while it is disabled.");
                        continue;
                    }
                }
                else if (session == JoinerSession)
                {
                    if (JoinerBattle!.Disable && JoinerBattle.DisabledSkill == selectedSkill.Name)
                    {
                        await session.SendMessageAsync($"You cannot use {JoinerBattle.DisabledSkill} while it is disabled.");
                        continue;
                    }
                }

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

    public async Task<bool?> AdministerBattle(ClientSession FirstSession, ClientSession SecondSession, string FirstChoice, string SecondChoice)
    {
        string FirstAction = FirstChoice.Split('|')[0].Trim();
        string SecondAction = SecondChoice.Split('|')[0].Trim();

        string FirstFollowUp = FirstChoice.Split('|')[1].Trim();
        string SecondFollowUp = SecondChoice.Split('|')[1].Trim();

        if (FirstChoice == "Pass")
        {
            await FirstSession.SendMessageAsync("You have passed your turn.");
            await SecondSession.SendMessageAsync($"{FirstSession.Username} has passed their turn.");
        }
        if (SecondChoice == "Pass")
        {
            await SecondSession.SendMessageAsync("You have passed your turn.");
            await FirstSession.SendMessageAsync($"{SecondSession.Username} has passed their turn.");
        }

        PokemonMaster OriginalFirstBattle;
        PokemonMaster OriginalSecondBattle;
        if (FirstSession == CreatorSession)
        {
            OriginalFirstBattle = CreatorBattle!;
            OriginalSecondBattle = JoinerBattle!;
        }
        else
        {
            OriginalFirstBattle = JoinerBattle!;
            OriginalSecondBattle = CreatorBattle!;
        }

        // Debugging
        Console.WriteLine($"[Battle] First Action: {FirstAction} - {FirstFollowUp}");
        Console.WriteLine($"[Battle] Second Action: {SecondAction} - {SecondFollowUp}");

        Console.WriteLine($"[Battle] First Session: {FirstSession.Username}");
        Console.WriteLine($"[Battle] Second Session: {SecondSession.Username}");

        Console.WriteLine($"{OriginalFirstBattle.Name} - {OriginalFirstBattle.Health}/{OriginalFirstBattle.MaxHealth}");
        Console.WriteLine($"{OriginalSecondBattle.Name} - {OriginalSecondBattle.Health}/{OriginalSecondBattle.MaxHealth}");

        // Administer Status Effects
        bool SkipFirst = await StatusEffects(FirstSession);

        Console.WriteLine($"[Battle] Skip First: {SkipFirst}");

        // First Action for switch for Joiner and Creator
        if (FirstAction == "Switch" && SkipFirst == false)
        {
            if (FirstSession == CreatorSession)
            {
                SwitchCure(FirstSession);
                // Switch Pokemon
                var SwitchTo = creatorPokemon!
                    .FirstOrDefault(p => p.Name == FirstFollowUp);

                creatorPokemon!.Add(CreatorBattle!);
                creatorPokemon.Remove(SwitchTo!);
                CreatorBattle = SwitchTo;
                await CreatorSession.SendMessageAsync($"\n{CreatorSession.Username} switched to {CreatorBattle!.Name}!");
                await JoinerSession.SendMessageAsync($"\n{CreatorSession.Username} switched to {CreatorBattle.Name}!");
            }
            else
            {
                SwitchCure(FirstSession);
                // Switch Pokemon
                var SwitchTo = joinerPokemon!
                    .FirstOrDefault(p => p.Name == FirstFollowUp);

                joinerPokemon!.Add(JoinerBattle!);
                joinerPokemon.Remove(SwitchTo!);
                JoinerBattle = SwitchTo;
                await JoinerSession.SendMessageAsync($"\n{JoinerSession.Username} switched to {JoinerBattle!.Name}!");
                await CreatorSession.SendMessageAsync($"\n{JoinerSession.Username} switched to {JoinerBattle.Name}!");
            }
        }

        // First Attack for joiner and creator
        if (FirstAction == "Attack" && SkipFirst == false)
        {
            if (FirstSession == CreatorSession)
            {
                var skill = CreatorBattle!.Skills.FirstOrDefault(s => s.Name == FirstFollowUp);

                await CreatorSession.SendMessageAsync($"\nYou used {skill!.Name}!\n");
                await JoinerSession.SendMessageAsync($"\n{CreatorSession.Username} used {skill.Name}!");

                if (skill != null)
                {
                    if (JoinerBattle!.Underground)
                    {
                        if (skill.Name == "Earthquake")
                        {
                            await skill.SkillEfect(JoinerBattle!, CreatorBattle, CreatorSession, JoinerSession);
                        }
                        else
                        {
                            await CreatorSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} used {skill.Name} but it missed because {JoinerBattle.Name} is underground!");
                            await JoinerSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} used {skill.Name} but it missed because {JoinerBattle.Name} is underground!");
                        }
                    }
                    else if (JoinerBattle.Flying)
                    {
                        if (skill.Name == "Thunder" || skill.Name == "Thunderbolt")
                        {
                            await skill.SkillEfect(JoinerBattle!, CreatorBattle, CreatorSession, JoinerSession);
                        }
                        else
                        {
                            await CreatorSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} used {skill.Name} but it missed because {JoinerBattle.Name} is flying!");
                            await JoinerSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} used {skill.Name} but it missed because {JoinerBattle.Name} is flying!");
                        }
                    }
                    else if (skill.Name == "Dig" && CreatorBattle.Dig)
                    {
                        await SkillHelper.ProcessDig(JoinerBattle, CreatorBattle!, CreatorSession, JoinerSession);
                    }
                    else if (skill.Name == "Fly" && CreatorBattle.Flying)
                    {
                        await SkillHelper.ProcessFly(JoinerBattle, CreatorBattle!, CreatorSession, JoinerSession);
                    }
                    else if (skill.Name == "Hyper Beam" && CreatorBattle.HyperBeamRecharge)
                    {
                        CreatorBattle.HyperBeamRecharge = false;
                        await CreatorSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} is recharging after using Hyper Beam!");
                        await JoinerSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} is recharging after using Hyper Beam!");
                    }
                    else if (skill.Name == "Roar")
                    {
                        if (joinerPokemon!.Count == 0)
                        {
                            await CreatorSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} used {skill.Name} but it failed!");
                            await JoinerSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} used {skill.Name} but it failed!");
                        }
                        else
                        {
                            await JoinerSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} used {skill.Name}, forcing {JoinerBattle.Name} to switch out!");
                            await CreatorSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} used {skill.Name}, forcing {JoinerBattle.Name} to switch out!");
                            bool? Timeout = await ForceSwitchPokemon(JoinerSession);
                            if (Timeout == false)
                            {
                                return false;
                            }
                            else if (Timeout == true)
                            {
                                return true;
                            }
                        }
                    }
                    else if (skill.Name == "Whirlwind")
                    {
                        if (joinerPokemon!.Count == 0)
                        {
                            await CreatorSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} used {skill.Name} but it failed!");
                            await JoinerSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} used {skill.Name} but it failed!");
                        }
                        else
                        {
                            await JoinerSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} used {skill.Name}, forcing {JoinerBattle.Name} to switch out!");
                            await CreatorSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} used {skill.Name}, forcing {JoinerBattle.Name} to switch out!");
                            bool? Timeout = await ForceSwitchPokemon(JoinerSession);
                            if (Timeout == false)
                            {
                                return false;
                            }
                            else if (Timeout == true)
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        await skill.SkillEfect(JoinerBattle!, CreatorBattle, CreatorSession, JoinerSession);
                    }

                    // Check if the target Pokémon fainted
                    if (JoinerBattle!.Health <= 0 || CreatorBattle!.Health <= 0)
                    {
                        var winner = await CheckStats();
                        if (winner == false)
                        {
                            return false;
                        }
                        else if (winner == true)
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                var skill = JoinerBattle!.Skills.FirstOrDefault(s => s.Name == FirstFollowUp);

                await JoinerSession.SendMessageAsync($"\nYou used {skill!.Name}!\n");
                await CreatorSession.SendMessageAsync($"\n{JoinerSession.Username} used {skill.Name}!");

                if (skill != null)
                {
                    if (CreatorBattle!.Underground)
                    {
                        if (skill.Name == "Earthquake")
                        {
                            await skill.SkillEfect(CreatorBattle!, JoinerBattle, JoinerSession, CreatorSession);
                        }
                        else
                        {
                            await JoinerSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} used {skill.Name} but it missed because {CreatorBattle.Name} is underground!");
                            await CreatorSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} used {skill.Name} but it missed because {CreatorBattle.Name} is underground!");
                        }
                    }
                    else if (CreatorBattle.Flying)
                    {
                        if (skill.Name == "Thunder" || skill.Name == "Thunderbolt")
                        {
                            await skill.SkillEfect(CreatorBattle!, JoinerBattle, JoinerSession, CreatorSession);
                        }
                        else
                        {
                            await JoinerSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} used {skill.Name} but it missed because {CreatorBattle.Name} is flying!");
                            await CreatorSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} used {skill.Name} but it missed because {CreatorBattle.Name} is flying!");
                        }
                    }
                    else if (skill.Name == "Dig" && JoinerBattle.Dig)
                    {
                        await SkillHelper.ProcessDig(CreatorBattle, JoinerBattle!, JoinerSession, CreatorSession);
                    }
                    else if (skill.Name == "Fly" && JoinerBattle.Flying)
                    {
                        await SkillHelper.ProcessFly(CreatorBattle, JoinerBattle!, JoinerSession, CreatorSession);
                    }
                    else if (skill.Name == "Hyper Beam" && JoinerBattle.HyperBeamRecharge)
                    {
                        JoinerBattle.HyperBeamRecharge = false;
                        await JoinerSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} is recharging after using Hyper Beam!");
                        await CreatorSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} is recharging after using Hyper Beam!");
                    }
                    else if (skill.Name == "Roar")
                    {
                        if (creatorPokemon!.Count == 0)
                        {
                            await JoinerSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} used {skill.Name} but it failed!");
                            await CreatorSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} used {skill.Name} but it failed!");
                        }
                        else
                        {
                            await CreatorSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} used {skill.Name}, forcing {CreatorBattle.Name} to switch out!");
                            await JoinerSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} used {skill.Name}, forcing {CreatorBattle.Name} to switch out!");
                            bool? Timeout = await ForceSwitchPokemon(CreatorSession);
                            if (Timeout == false)
                            {
                                return false;
                            }
                            else if (Timeout == true)
                            {
                                return true;
                            }
                        }
                    }
                    else if (skill.Name == "Whirlwind")
                    {
                        if (creatorPokemon!.Count == 0)
                        {
                            await JoinerSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} used {skill.Name} but it failed!");
                            await CreatorSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} used {skill.Name} but it failed!");
                        }
                        else
                        {
                            await CreatorSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} used {skill.Name}, forcing {CreatorBattle.Name} to switch out!");
                            await JoinerSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} used {skill.Name}, forcing {CreatorBattle.Name} to switch out!");
                            bool? Timeout = await ForceSwitchPokemon(CreatorSession);
                            if (Timeout == false)
                            {
                                return false;
                            }
                            else if (Timeout == true)
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        await skill.SkillEfect(CreatorBattle!, JoinerBattle, JoinerSession, CreatorSession);
                    }

                    if (JoinerBattle!.Health <= 0 || CreatorBattle!.Health <= 0)
                    {
                        var winner = await CheckStats();
                        if (winner == false)
                        {
                            return false;
                        }
                        else if (winner == true)
                        {
                            return true;
                        }
                    }
                }
            }
        }

        // Second Action
        if ((OriginalSecondBattle == JoinerBattle && SecondSession == JoinerSession) || (OriginalSecondBattle == CreatorBattle && SecondSession == CreatorSession))
        {
            // Second Action Switch for creator and joiner
            if (SecondAction == "Switch")
            {
                // Administer Status Effects
                bool SkipSecond = await StatusEffects(SecondSession);

                if (SecondSession == CreatorSession && SkipSecond == false)
                {
                    // Switch Pokemon
                    SwitchCure(SecondSession);
                    var SwitchTo = creatorPokemon!
                        .FirstOrDefault(p => p.Name == SecondFollowUp);

                    creatorPokemon!.Add(CreatorBattle!);
                    creatorPokemon.Remove(SwitchTo!);
                    CreatorBattle = SwitchTo;
                    await CreatorSession.SendMessageAsync($"\n{CreatorSession.Username} switched to {CreatorBattle!.Name}!");
                    await JoinerSession.SendMessageAsync($"\n{CreatorSession.Username} switched to {CreatorBattle.Name}!");
                }
                else if (SkipSecond == false)
                {
                    // Switch Pokemon
                    SwitchCure(SecondSession);
                    var SwitchTo = joinerPokemon!
                        .FirstOrDefault(p => p.Name == SecondFollowUp);

                    joinerPokemon!.Add(JoinerBattle!);
                    joinerPokemon.Remove(SwitchTo!);
                    JoinerBattle = SwitchTo;
                    await JoinerSession.SendMessageAsync($"\n{JoinerSession.Username} switched to {JoinerBattle!.Name}!");
                    await CreatorSession.SendMessageAsync($"\n{JoinerSession.Username} switched to {JoinerBattle.Name}!");
                }
            }

            // Second Action Attack
            if (SecondAction == "Attack")
            {
                // Administer Status Effects
                bool SkipSecond = await StatusEffects(SecondSession);

                if (SecondSession == CreatorSession && SkipSecond == false)
                {
                    var skill = CreatorBattle!.Skills.FirstOrDefault(s => s.Name == SecondFollowUp);

                    await CreatorSession.SendMessageAsync($"\nYou used {skill!.Name}!\n");
                    await JoinerSession.SendMessageAsync($"\n{CreatorSession.Username} used {skill.Name}!");

                    if (skill != null)
                    {
                        if (JoinerBattle!.Underground)
                        {
                            if (skill.Name == "Earthquake")
                            {
                                await skill.SkillEfect(JoinerBattle!, CreatorBattle, CreatorSession, JoinerSession);
                            }
                            else
                            {
                                await CreatorSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} used {skill.Name} but it missed because {JoinerBattle.Name} is underground!");
                                await JoinerSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} used {skill.Name} but it missed because {JoinerBattle.Name} is underground!");
                            }
                        }
                        else if (JoinerBattle.Flying)
                        {
                            if (skill.Name == "Thunder" || skill.Name == "Thunderbolt")
                            {
                                await skill.SkillEfect(JoinerBattle!, CreatorBattle, CreatorSession, JoinerSession);
                            }
                            else
                            {
                                await CreatorSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} used {skill.Name} but it missed because {JoinerBattle.Name} is flying!");
                                await JoinerSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} used {skill.Name} but it missed because {JoinerBattle.Name} is flying!");
                            }
                        }
                        else if (skill.Name == "Dig" && CreatorBattle.Dig)
                        {
                            await SkillHelper.ProcessDig(JoinerBattle, CreatorBattle!, CreatorSession, JoinerSession);
                        }
                        else if (skill.Name == "Fly" && CreatorBattle.Flying)
                        {
                            await SkillHelper.ProcessFly(JoinerBattle, CreatorBattle!, CreatorSession, JoinerSession);
                        }
                        else if (skill.Name == "Hyper Beam" && CreatorBattle.HyperBeamRecharge)
                        {
                            CreatorBattle.HyperBeamRecharge = false;
                            await CreatorSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} is recharging after using Hyper Beam!");
                            await JoinerSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} is recharging after using Hyper Beam!");
                        }
                        else if (skill.Name == "Roar")
                        {
                            if (joinerPokemon!.Count == 0)
                            {
                                await CreatorSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} used {skill.Name} but it failed!");
                                await JoinerSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} used {skill.Name} but it failed!");
                            }
                            else
                            {
                                await JoinerSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} used {skill.Name}, forcing {JoinerBattle.Name} to switch out!");
                                await CreatorSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} used {skill.Name}, forcing {JoinerBattle.Name} to switch out!");
                                bool? Timeout = await ForceSwitchPokemon(JoinerSession);
                                if (Timeout == false)
                                {
                                    return false;
                                }
                                else if (Timeout == true)
                                {
                                    return true;
                                }
                            }
                        }
                        else if (skill.Name == "Whirlwind")
                        {
                            if (joinerPokemon!.Count == 0)
                            {
                                await CreatorSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} used {skill.Name} but it failed!");
                                await JoinerSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} used {skill.Name} but it failed!");
                            }
                            else
                            {
                                await JoinerSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} used {skill.Name}, forcing {JoinerBattle.Name} to switch out!");
                                await CreatorSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} used {skill.Name}, forcing {JoinerBattle.Name} to switch out!");
                                bool? Timeout = await ForceSwitchPokemon(JoinerSession);
                                if (Timeout == false)
                                {
                                    return false;
                                }
                                else if (Timeout == true)
                                {
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            await skill.SkillEfect(JoinerBattle!, CreatorBattle, CreatorSession, JoinerSession);
                        }

                        if (JoinerBattle!.Health <= 0 || CreatorBattle!.Health <= 0)
                        {
                            var winner = await CheckStats();
                            if (winner == false)
                            {
                                return false;
                            }
                            else if (winner == true)
                            {
                                return true;
                            }
                        }
                    }
                }
                else if (SkipSecond == false)
                {
                    var skill = JoinerBattle!.Skills.FirstOrDefault(s => s.Name == SecondFollowUp);

                    await CreatorSession.SendMessageAsync($"\nYou used {skill!.Name}!\n");
                    await JoinerSession.SendMessageAsync($"\n{JoinerSession.Username} used {skill.Name}!");
                    if (skill != null)
                    {
                        if (CreatorBattle!.Underground)
                        {
                            if (skill.Name == "Earthquake")
                            {
                                await skill.SkillEfect(CreatorBattle!, JoinerBattle, JoinerSession, CreatorSession);
                            }
                            else
                            {
                                await JoinerSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} used {skill.Name} but it missed because {CreatorBattle.Name} is underground!");
                                await CreatorSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} used {skill.Name} but it missed because {CreatorBattle.Name} is underground!");
                            }
                        }
                        else if (CreatorBattle.Flying)
                        {
                            if (skill.Name == "Thunder" || skill.Name == "Thunderbolt")
                            {
                                await skill.SkillEfect(CreatorBattle!, JoinerBattle, JoinerSession, CreatorSession);
                            }
                            else
                            {
                                await JoinerSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} used {skill.Name} but it missed because {CreatorBattle.Name} is flying!");
                                await CreatorSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} used {skill.Name} but it missed because {CreatorBattle.Name} is flying!");
                            }
                        }
                        else if (skill.Name == "Dig" && JoinerBattle.Dig)
                        {
                            await SkillHelper.ProcessDig(CreatorBattle, JoinerBattle!, JoinerSession, CreatorSession);
                        }
                        else if (skill.Name == "Fly" && JoinerBattle.Flying)
                        {
                            await SkillHelper.ProcessFly(CreatorBattle, JoinerBattle!, JoinerSession, CreatorSession);
                        }
                        else if (skill.Name == "Hyper Beam" && JoinerBattle.HyperBeamRecharge)
                        {
                            JoinerBattle.HyperBeamRecharge = false;
                            await JoinerSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} is recharging after using Hyper Beam!");
                            await CreatorSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} is recharging after using Hyper Beam!");
                        }
                        else if (skill.Name == "Roar")
                        {
                            if (creatorPokemon!.Count == 0)
                            {
                                await JoinerSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} used {skill.Name} but it failed!");
                                await CreatorSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} used {skill.Name} but it failed!");
                            }
                            else
                            {
                                await CreatorSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} used {skill.Name}, forcing {CreatorBattle.Name} to switch out!");
                                await JoinerSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} used {skill.Name}, forcing {CreatorBattle.Name} to switch out!");
                                bool? Timeout = await ForceSwitchPokemon(CreatorSession);
                                if (Timeout == false)
                                {
                                    return false;
                                }
                                else if (Timeout == true)
                                {
                                    return true;
                                }
                            }
                        }
                        else if (skill.Name == "Whirlwind")
                        {
                            if (creatorPokemon!.Count == 0)
                            {
                                await JoinerSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} used {skill.Name} but it failed!");
                                await CreatorSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} used {skill.Name} but it failed!");
                            }
                            else
                            {
                                await CreatorSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} used {skill.Name}, forcing {CreatorBattle.Name} to switch out!");
                                await JoinerSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} used {skill.Name}, forcing {CreatorBattle.Name} to switch out!");
                                bool? Timeout = await ForceSwitchPokemon(CreatorSession);
                                if (Timeout == false)
                                {
                                    return false;
                                }
                                else if (Timeout == true)
                                {
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            await skill.SkillEfect(CreatorBattle!, JoinerBattle, JoinerSession, CreatorSession);
                        }

                        if (JoinerBattle!.Health <= 0 || CreatorBattle!.Health <= 0)
                        {
                            var winner = await CheckStats();
                            if (winner == false)
                            {
                                return false;
                            }
                            else if (winner == true)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }

        if (JoinerBattle!.Health <= 0 || CreatorBattle!.Health <= 0)
        {
            var winner = await CheckStats();
            if (winner == false)
            {
                return false;
            }
            else if (winner == true)
            {
                return true;
            }
        }

        // Reset Flinch
        if (CreatorBattle!.Flinch)
        {
            CreatorBattle.Flinch = false;
        }

        if (JoinerBattle!.Flinch)
        {
            JoinerBattle.Flinch = false;
        }

        // Disable End
        if (CreatorBattle.Disable)
        {
            CreatorBattle.DisableTurns -= 1;
            if (CreatorBattle.DisableTurns == 0)
            {
                CreatorBattle.Disable = false;
                await CreatorSession.SendMessageAsync($"\nYour {CreatorBattle.Name} can once again use {CreatorBattle.DisabledSkill}.");
                CreatorBattle.DisabledSkill = string.Empty;
            }
        }

        if (JoinerBattle!.Disable)
        {
            JoinerBattle.DisableTurns -= 1;
            if (JoinerBattle.DisableTurns == 0)
            {
                JoinerBattle.Disable = false;
                await JoinerSession.SendMessageAsync($"\nYour {JoinerBattle.Name} can once again use {JoinerBattle.DisabledSkill}.");
                JoinerBattle.DisabledSkill = string.Empty;
            }
        }

        

        return null;

    }

    public async Task<bool> StatusEffects(ClientSession session)
    {
        bool currentSkip = false;

        if (session == CreatorSession && CreatorBattle != null)
        {
            // Flinch
            if (CreatorBattle.Flinch)
            {
                currentSkip = true;
                await CreatorSession.SendMessageAsync($"\nYour {CreatorBattle.Name} flinched and couldn't move!");
                await JoinerSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} flinched and couldn't move!");
            }

            // Bide
            if (CreatorBattle.BideActive)
            {
                currentSkip = true;
                CreatorBattle.BideTurns -= 1;
                if (CreatorBattle.BideTurns == 0)
                {
                    CreatorBattle.BideActive = false;
                    await CreatorSession.SendMessageAsync($"\nYour {CreatorBattle.Name} can once again move.");

                    if (JoinerBattle!.Substitude)
                    {
                        if (JoinerBattle.SubstituteHealth <= CreatorBattle.BideDamage)
                        {
                            JoinerBattle.Substitude = false;
                            JoinerBattle.SubstituteHealth = 0;

                            await CreatorSession.SendMessageAsync($"Your {CreatorBattle.Name} used Wing Attack and broke {JoinerBattle.Name}'s Substitute!");
                            await JoinerSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} used Wing Attack and broke your {JoinerBattle.Name}'s Substitute!");
                        }
                        else
                        {
                            JoinerBattle.SubstituteHealth -= CreatorBattle.BideDamage;

                            await CreatorSession.SendMessageAsync($"Your {CreatorBattle.Name} used Wing Attack on {JoinerBattle.Name}'s Substitute, dealing {CreatorBattle.BideDamage:F1} damage.");
                            await JoinerSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} used Wing Attack on your {JoinerBattle.Name}'s Substitute, dealing {CreatorBattle.BideDamage:F1} damage.");

                            if (JoinerBattle.SubstituteHealth < 0) JoinerBattle.SubstituteHealth = 0;
                        }
                    }
                    else
                    {
                        JoinerBattle.Health -= CreatorBattle.BideDamage;

                        await CreatorSession.SendMessageAsync($"Your {CreatorBattle.Name} used Wing Attack on {JoinerBattle.Name}, dealing {CreatorBattle.BideDamage:F1} damage!");
                        await JoinerSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} used Wing Attack on your {JoinerBattle.Name}, dealing {CreatorBattle.BideDamage:F1} damage!");
                    }
                    CreatorBattle.BideDamage = 0;
                }
            }

            // Paralyze
            if (CreatorBattle.Paralyzed && Random.Shared.NextDouble() <= 0.25)
            {
                await CreatorSession.SendMessageAsync($"\nYour {CreatorBattle.Name} is paralyzed and couldn't move!");
                await JoinerSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} is paralyzed and couldn't move!");
                currentSkip = true;
            }

            // Confusion
            if (CreatorBattle.Confused)
            {
                CreatorBattle.ConfusionTurns -= 1;
                if (Random.Shared.NextDouble() <= 0.5)
                {
                    currentSkip = true;
                    await CreatorSession.SendMessageAsync($"\nYour {CreatorBattle.Name} is confused and hurt itself!");
                    await JoinerSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} is confused and hurt itself!");
                    CreatorBattle.Health -= CreatorBattle.MaxHealth * 0.4f;
                }
            }

            // Burn
            if (CreatorBattle.Burning)
            {
                CreatorBattle.Health -= CreatorBattle.MaxHealth * (1.0f / 16.0f);
                await CreatorSession.SendMessageAsync($"\nYour {CreatorBattle.Name} is burned and lost {CreatorBattle.MaxHealth * (1.0f / 16.0f):F1} HP!");
                await JoinerSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} is burned and lost {CreatorBattle.MaxHealth * (1.0f / 16.0f):F1} HP!");
            }

            // Poison
            if (CreatorBattle.Poisoned)
            {
                float damage = CreatorBattle.MaxHealth * (1.0f / 16.0f);
                CreatorBattle.Health -= damage;
                await CreatorSession.SendMessageAsync($"\nYour {CreatorBattle.Name} is poisoned and lost {damage:F1} HP!");
                await JoinerSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} is poisoned and lost {damage:F1} HP!");
            }

            // Badly Poison
            if (CreatorBattle.BadlyPoisoned)
            {
                CreatorBattle.BadlyPoisonedTurns += 1;
                float damage = CreatorBattle.MaxHealth * (1.0f / 16.0f) * CreatorBattle.BadlyPoisonedTurns;
                CreatorBattle.Health -= damage;
                await CreatorSession.SendMessageAsync($"\nYour {CreatorBattle.Name} is badly poisoned and lost {damage:F1} HP!");
                await JoinerSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} is badly poisoned and lost {damage:F1} HP!");
            }

            // Frozen
            if (CreatorBattle.Freezing)
            {
                currentSkip = true;
                await CreatorSession.SendMessageAsync($"\nYour {CreatorBattle.Name} is frozen and couldn't move!");
                await JoinerSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} is frozen and couldn't move!");
            }

            // Sleeping
            if (CreatorBattle.Sleeping)
            {
                currentSkip = true;
                await CreatorSession.SendMessageAsync($"\nYour {CreatorBattle.Name} is asleep and couldn't move!");
                await JoinerSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} is asleep and couldn't move!");
                CreatorBattle.SleepTurns -= 1;
                if (CreatorBattle.SleepTurns == 0)
                {
                    CreatorBattle.Sleeping = false;
                    await CreatorSession.SendMessageAsync($"\nYour {CreatorBattle.Name} woke up!");
                    await JoinerSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} woke up!");
                    if (CreatorBattle.Rest)
                    {
                        CreatorBattle.Rest = false;
                        CreatorBattle.Health = CreatorBattle.MaxHealth;

                        CreatorBattle.Freezing = false;
                        CreatorBattle.Poisoned = false;
                        CreatorBattle.BadlyPoisoned = false;
                        CreatorBattle.BadlyPoisonedTurns = 0;

                        if (CreatorBattle.Burning)
                        {
                            CreatorBattle.BurningAttack = false;
                            CreatorBattle.Burning = false;
                            CreatorBattle.Attack *= 2;
                        }

                        if (CreatorBattle.Paralyzed)
                        {
                            CreatorBattle.Paralyzed = false;
                            CreatorBattle.ParalyzeSpeed = false;
                            CreatorBattle.Speed *= 2;
                        }

                        await CreatorSession.SendMessageAsync($"\nYour {CreatorBattle.Name} restored all of its health after resting!");
                        await JoinerSession.SendMessageAsync($"{CreatorSession.Username}'s {CreatorBattle.Name} restored all of its health after resting!");
                    }
                }
            }

            // Leech Seed
            if (CreatorBattle.LeechSeed)
            {
                CreatorBattle.Health -= CreatorBattle.MaxHealth * (1.0f / 16.0f);
                JoinerBattle!.Health += CreatorBattle.MaxHealth * (1.0f / 16.0f);
                await CreatorSession.SendMessageAsync($"\nYour {CreatorBattle.Name} is being leeched health and lost {CreatorBattle.MaxHealth * (1.0f / 16.0f):F1} HP!");
                await JoinerSession.SendMessageAsync($"\nYour's {JoinerBattle.Name} is leeching health and gained {CreatorBattle.MaxHealth * (1.0f / 16.0f):F1} HP!");

                CreatorBattle.LeechSeedTurns -= 1;
                if (CreatorBattle.LeechSeedTurns == 0)
                {
                    CreatorBattle.LeechSeed = false;
                    await CreatorSession.SendMessageAsync($"\nYour {CreatorBattle.Name} is no longer being leeched health.");
                }
            }

            // Light Screen
            if (CreatorBattle.LightScreen)
            {
                CreatorBattle.LightScreenTurns -= 1;
                if (CreatorBattle.LightScreenTurns == 0)
                {
                    CreatorBattle.LightScreen = false;
                    await CreatorSession.SendMessageAsync($"\nYour {CreatorBattle.Name}'s Light Screen has faded.");
                }
            }

            // Mist
            if (CreatorBattle.Mist)
            {
                CreatorBattle.MistTurns -= 1;
                if (CreatorBattle.MistTurns == 0)
                {
                    CreatorBattle.Mist = false;
                    await CreatorSession.SendMessageAsync($"\nYour {CreatorBattle.Name}'s Mist has faded.");
                }
            }


            // Check if dead
            if (CreatorBattle.Health <= 0)
            {
                await CheckStats();
                currentSkip = true;
            }

        }
        else if (session == JoinerSession && JoinerBattle != null)
        {
            // Flinch
            if (JoinerBattle.Flinch)
            {
                currentSkip = true;
                await JoinerSession.SendMessageAsync($"\nYour {JoinerBattle.Name} flinched and couldn't move!");
                await CreatorSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} flinched and couldn't move!");
            }

            // Bide
            if (JoinerBattle.BideActive)
            {
                currentSkip = true;
                JoinerBattle.BideTurns -= 1;
                if (JoinerBattle.BideTurns == 0)
                {
                    JoinerBattle.BideActive = false;
                    await JoinerSession.SendMessageAsync($"\nYour {JoinerBattle.Name} can once again move.");

                    if (CreatorBattle!.Substitude)
                    {
                        if (CreatorBattle.SubstituteHealth <= JoinerBattle.BideDamage)
                        {
                            CreatorBattle.Substitude = false;
                            CreatorBattle.SubstituteHealth = 0;

                            await JoinerSession.SendMessageAsync($"Your {JoinerBattle.Name} used Wing Attack and broke {CreatorBattle.Name}'s Substitute!");
                            await CreatorSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} used Wing Attack and broke your {CreatorBattle.Name}'s Substitute!");
                        }
                        else
                        {
                            CreatorBattle.SubstituteHealth -= JoinerBattle.BideDamage;

                            await JoinerSession.SendMessageAsync($"Your {JoinerBattle.Name} used Wing Attack on {CreatorBattle.Name}'s Substitute, dealing {JoinerBattle.BideDamage:F1} damage.");
                            await CreatorSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} used Wing Attack on your {CreatorBattle.Name}'s Substitute, dealing {JoinerBattle.BideDamage:F1} damage.");

                            if (CreatorBattle.SubstituteHealth < 0) CreatorBattle.SubstituteHealth = 0;
                        }
                    }
                    else
                    {
                        CreatorBattle.Health -= JoinerBattle.BideDamage;

                        await JoinerSession.SendMessageAsync($"Your {JoinerBattle.Name} used Wing Attack on {CreatorBattle.Name}, dealing {JoinerBattle.BideDamage:F1} damage!");
                        await CreatorSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} used Wing Attack on your {CreatorBattle.Name}, dealing {JoinerBattle.BideDamage:F1} damage!");
                    }
                    JoinerBattle.BideDamage = 0;
                }
            }

            // Paralyze
            if (JoinerBattle.Paralyzed && Random.Shared.NextDouble() <= 0.25)
            {
                await JoinerSession.SendMessageAsync($"\nYour {JoinerBattle.Name} is paralyzed and couldn't move!");
                await CreatorSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} is paralyzed and couldn't move!");
                currentSkip = true;
            }

            // Confusion
            if (JoinerBattle.Confused)
            {
                JoinerBattle.ConfusionTurns -= 1;
                if (Random.Shared.NextDouble() <= 0.50)
                {
                    await JoinerSession.SendMessageAsync($"\nYour {JoinerBattle.Name} is confused and hurt itself!");
                    await CreatorSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} is confused and hurt itself!");
                    JoinerBattle.Health -= JoinerBattle.MaxHealth * 0.4f;
                    currentSkip = true;
                }
            }

            // Burn
            if (JoinerBattle.Burning)
            {
                JoinerBattle.Health -= JoinerBattle.MaxHealth * (1.0f / 16.0f);
                await JoinerSession.SendMessageAsync($"\nYour {JoinerBattle.Name} is burned and lost {JoinerBattle.MaxHealth * (1.0f / 16.0f):F1} HP!");
                await CreatorSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} is burned and lost {JoinerBattle.MaxHealth * (1.0f / 16.0f):F1} HP!");
            }

            // Poison
            if (JoinerBattle.Poisoned)
            {
                float damage = JoinerBattle.MaxHealth * (1.0f / 16.0f);
                JoinerBattle.Health -= damage;
                await JoinerSession.SendMessageAsync($"\nYour {JoinerBattle.Name} is poisoned and lost {damage:F1} HP!");
                await CreatorSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} is poisoned and lost {damage:F1} HP!");
            }

            // Badly Poison
            if (JoinerBattle.BadlyPoisoned)
            {
                JoinerBattle.BadlyPoisonedTurns += 1;
                float damage = JoinerBattle.MaxHealth * (1.0f / 16.0f) * JoinerBattle.BadlyPoisonedTurns;
                JoinerBattle.Health -= damage;
                await JoinerSession.SendMessageAsync($"\nYour {JoinerBattle.Name} is badly poisoned and lost {damage:F1} HP!");
                await CreatorSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} is badly poisoned and lost {damage:F1} HP!");
            }

            // Frozen
            if (JoinerBattle.Freezing)
            {
                currentSkip = true;
                await JoinerSession.SendMessageAsync($"\nYour {JoinerBattle.Name} is frozen and couldn't move!");
                await CreatorSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} is frozen and couldn't move!");
            }

            // Sleeping
            if (JoinerBattle.Sleeping)
            {
                currentSkip = true;
                await JoinerSession.SendMessageAsync($"\nYour {JoinerBattle.Name} is asleep and couldn't move!");
                await CreatorSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} is asleep and couldn't move!");
                JoinerBattle.SleepTurns -= 1;
                if (JoinerBattle.SleepTurns == 0)
                {
                    JoinerBattle.Sleeping = false;
                    await JoinerSession.SendMessageAsync($"\nYour {JoinerBattle.Name} woke up!");
                    await CreatorSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} woke up!");
                    if (JoinerBattle.Rest)
                    {
                        JoinerBattle.Rest = false;
                        JoinerBattle.Health = JoinerBattle.MaxHealth;

                        JoinerBattle.Freezing = false;
                        JoinerBattle.Poisoned = false;
                        JoinerBattle.BadlyPoisoned = false;
                        JoinerBattle.BadlyPoisonedTurns = 0;
                        JoinerBattle.Confused = false;
                        if (JoinerBattle.Burning)
                        {
                            JoinerBattle.BurningAttack = false;
                            JoinerBattle.Burning = false;
                            JoinerBattle.Attack *= 2;
                        }

                        if (JoinerBattle.Paralyzed)
                        {
                            JoinerBattle.Paralyzed = false;
                            JoinerBattle.ParalyzeSpeed = false;
                            JoinerBattle.Speed *= 2;
                        }
                        await JoinerSession.SendMessageAsync($"\nYour {JoinerBattle.Name} restored all of its health after resting!");
                        await CreatorSession.SendMessageAsync($"{JoinerSession.Username}'s {JoinerBattle.Name} restored all of its health after resting!");
                    }
                }
            }

            // Leech Seed
            if (JoinerBattle.LeechSeed)
            {
                JoinerBattle.Health -= JoinerBattle.MaxHealth * (1.0f / 16.0f);
                CreatorBattle!.Health += JoinerBattle.MaxHealth * (1.0f / 16.0f);
                await JoinerSession.SendMessageAsync($"\nYour {JoinerBattle.Name} is being leeched health and lost {JoinerBattle.MaxHealth * (1.0f / 16.0f):F1} HP!");
                await CreatorSession.SendMessageAsync($"\nYour's {CreatorBattle.Name} is leeching health and gained {JoinerBattle.MaxHealth * (1.0f / 16.0f):F1} HP!");

                JoinerBattle.LeechSeedTurns -= 1;
                if (JoinerBattle.LeechSeedTurns == 0)
                {
                    JoinerBattle.LeechSeed = false;
                    await JoinerSession.SendMessageAsync($"\nYour {JoinerBattle.Name} is no longer being leeched health.");
                }
            }

            // Light Screen
            if (JoinerBattle.LightScreen)
            {
                JoinerBattle.LightScreenTurns -= 1;
                if (JoinerBattle.LightScreenTurns == 0)
                {
                    JoinerBattle.LightScreen = false;
                    await JoinerSession.SendMessageAsync($"\nYour {JoinerBattle.Name}'s Light Screen has faded.");
                }
            }

            // Mist
            if (JoinerBattle.Mist)
            {
                JoinerBattle.MistTurns -= 1;
                if (JoinerBattle.MistTurns == 0)
                {
                    JoinerBattle.Mist = false;
                    await JoinerSession.SendMessageAsync($"\nYour {JoinerBattle.Name}'s Mist has faded.");
                }
            }

            // Check if dead
            if (JoinerBattle.Health <= 0)
            {
                await CheckStats();
                currentSkip = true;
            }
        }
        return currentSkip;
    }

    public void SwitchCure(ClientSession session)
    {
        if (session == CreatorSession && CreatorBattle != null)
        {
            CreatorBattle.Freezing = false;
            CreatorBattle.Poisoned = false;
            CreatorBattle.BadlyPoisoned = false;
            CreatorBattle.Confused = false;

            if (CreatorBattle.Burning)
            {
                CreatorBattle.BurningAttack = false;
                CreatorBattle.Burning = false;
                CreatorBattle.Attack *= 2;
            }

            if (CreatorBattle.Paralyzed)
            {
                CreatorBattle.Paralyzed = false;
                CreatorBattle.ParalyzeSpeed = false;
                CreatorBattle.Speed *= 2;
            }

        }
        else if (session == JoinerSession && JoinerBattle != null)
        {
            JoinerBattle.Freezing = false;
            JoinerBattle.Poisoned = false;
            JoinerBattle.BadlyPoisoned = false;
            JoinerBattle.BadlyPoisonedTurns = 0;
            JoinerBattle.Confused = false;

            if (JoinerBattle.Burning)
            {
                JoinerBattle.BurningAttack = false;
                JoinerBattle.Burning = false;
                JoinerBattle.Attack *= 2;
            }

            if (JoinerBattle.Paralyzed)
            {
                JoinerBattle.Paralyzed = false;
                JoinerBattle.ParalyzeSpeed = false;
                JoinerBattle.Speed *= 2;
            }
        }
    }

    public async Task<bool?> ForceSwitchPokemon(ClientSession session)
    {
        if (session == JoinerSession)
        {
            var joinerResponse = Task.Run(async () =>
            {
                while (true)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("════════════ POKÉMON AVAILABLE ════════════");

                    int i = 1;
                    var pokemonCollection = CreatorSession == session ? creatorPokemon : joinerPokemon;
                    var pokedict = new Dictionary<int, string>();

                    var BattlePokemon = CreatorSession == session ? CreatorBattle : JoinerBattle;

                    foreach (var pokemon in pokemonCollection!)
                    {
                        sb.Append($"\n [{i}] {pokemon.Name} - HP: {pokemon.Health}/{pokemon.MaxHealth}");
                        pokedict.Add(i, pokemon.Name!);
                        i++;
                    }

                    await session.SendMessageAsync(sb.ToString());

                    string choice = await session.GetInputAsync("\nChoice:");

                    if (int.TryParse(choice, out int choiceNumber) && choiceNumber >= 1 && choiceNumber <= pokemonCollection.Count)
                    {
                        var selectedPokemon = pokemonCollection
                            .FirstOrDefault(p => p.Name == pokedict[choiceNumber]);

                        SwitchCure(JoinerSession);
                        // Switch Pokemon
                        var SwitchTo = joinerPokemon!
                            .FirstOrDefault(p => p.Name == selectedPokemon!.Name);

                        joinerPokemon!.Add(JoinerBattle!);
                        joinerPokemon.Remove(SwitchTo!);
                        JoinerBattle = SwitchTo;
                        break;
                    }
                    else
                    {
                        await session.SendMessageAsync("Invalid choice. Please try again.");
                        continue;
                    }
                }
            });

            var CreatorResponse = Task.Run(async () =>
            {
                await CreatorSession.SendMessageAsync($"\nPlease wait for {JoinerSession.Username} to switch Pokemon.");
            });

            var timeout = Task.Delay(60000); // 60 seconds timeout

            var BothTasks = Task.WhenAll(joinerResponse, CreatorResponse);
            var completedTask = await Task.WhenAny(BothTasks, timeout);

            if (completedTask == timeout)
            {
                await CreatorSession.SendMessageAsync($"Time's up! {JoinerSession.Username} didn't respond in time.");
                await JoinerSession.SendMessageAsync($"Time's up! You didn't respond in time.");
                return false;
            }
            else
            {
                await BothTasks; // Await both tasks to ensure they complete
                return null;
            }
        }
        if (session == CreatorSession)
        {
            var creatorResponse = Task.Run(async () =>
            {
                while (true)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("════════════ POKÉMON AVAILABLE ════════════");

                    int i = 1;
                    var pokemonCollection = CreatorSession == session ? creatorPokemon : joinerPokemon;
                    var pokedict = new Dictionary<int, string>();

                    var BattlePokemon = CreatorSession == session ? CreatorBattle : JoinerBattle;

                    foreach (var pokemon in pokemonCollection!)
                    {
                        sb.Append($"\n [{i}] {pokemon.Name} - HP: {pokemon.Health}/{pokemon.MaxHealth}");
                        pokedict.Add(i, pokemon.Name!);
                        i++;
                    }

                    await session.SendMessageAsync(sb.ToString());

                    string choice = await session.GetInputAsync("\nChoice:");

                    if (int.TryParse(choice, out int choiceNumber) && choiceNumber >= 1 && choiceNumber <= pokemonCollection.Count)
                    {
                        var selectedPokemon = pokemonCollection
                            .FirstOrDefault(p => p.Name == pokedict[choiceNumber]);

                        SwitchCure(CreatorSession);
                        // Switch Pokemon
                        var SwitchTo = creatorPokemon!
                            .FirstOrDefault(p => p.Name == selectedPokemon!.Name);

                        creatorPokemon!.Add(CreatorBattle!);
                        creatorPokemon.Remove(SwitchTo!);
                        CreatorBattle = SwitchTo;
                        break;
                    }
                    else
                    {
                        await session.SendMessageAsync("Invalid choice. Please try again.");
                        continue;
                    }
                }
            });

            var JoinerResponse = Task.Run(async () =>
            {
                await JoinerSession.SendMessageAsync($"Please wait for {CreatorSession.Username} to switch Pokemon.");
            });

            var timeout = Task.Delay(60000); // 60 seconds timeout

            var BothTasks = Task.WhenAll(creatorResponse, JoinerResponse);
            var completedTask = await Task.WhenAny(BothTasks, timeout);

            if (completedTask == timeout)
            {
                await CreatorSession.SendMessageAsync($"Time's up! {JoinerSession.Username} didn't respond in time.");
                await JoinerSession.SendMessageAsync($"Time's up! You didn't respond in time.");
                return false;
            }
            else
            {
                await BothTasks; // Await both tasks to ensure they complete
                return null;
            }
        }
        return null;
    }
}