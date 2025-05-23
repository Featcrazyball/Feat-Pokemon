using Models;
using Server;
using Database;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Concurrent;
using Arena;

namespace Server;

public class GameRoom
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? Name { get; set; }
    public User? Host { get; set; }
    public User? Challenger { get; set; }
    public ClientSession? HostSession { get; set; }
    public ClientSession? ChallengerSession { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsFull => Challenger != null;
}

public class Game
{
    // Static collection to track all active rooms across sessions
    private static ConcurrentDictionary<string, GameRoom> _activeRooms = new ConcurrentDictionary<string, GameRoom>();
    
    // Add a new field to track rooms that are currently in battle
    private static ConcurrentDictionary<string, bool> _roomsInBattle = new ConcurrentDictionary<string, bool>();

    public static async Task Rooms(ClientSession session)
    {
        User user;
        using (var context = new DatabaseContext())
        {
            user = context.Users.FirstOrDefault(u => u.Username == session.Username)!;
        }

        if (user == null)
        {
            await session.SendMessageAsync("There has been an error locating your account. Please try again.");
            return;
        }
        
        while (true)
        {
            // Get only rooms that aren't full
            var availableRooms = _activeRooms.Values.Where(r => !r.IsFull && r.IsActive).ToList();

            if (user.InRoom)
            {
                while (true)
                {
                    if (user.InRoom == false)
                    {
                        break;
                    }
                }
            }

            StringBuilder sendMessage = new StringBuilder();
            sendMessage.Append(@$"
╔══════════════════════════════════════════════════════════════════════════════╗
║                                                                              ║
║               🏆🎮🎯    BATTLE ARENA ROOMS    🏆🎮🎯                         ║
║                                                                              ║
╠══════════════════════════════════════════════════════════════════════════════╣
║                                                                              ║");

            if (availableRooms.Count == 0)
            {
                sendMessage.Append($"\n║    No rooms available.                                                       ║");
                sendMessage.Append($"\n║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━          ║");
            }
            else
            {
                for (int i = 0; i < availableRooms.Count; i++)
                {
                    var room = availableRooms[i];
                    string roomDisplay = $"Room: {room.Name} - Host: {room.Host!.Username}";
                    
                    sendMessage.Append($"\n║    {$"[{i + 1}]",-5} {roomDisplay,-67} ║");
                    sendMessage.Append($"\n║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━          ║");
                }
            }

            // Add menu options
            sendMessage.Append("\n╠══════════════════════════════════════════════════════════════════════════════╣\n");
            sendMessage.Append("║                                                                              ║\n");
            sendMessage.Append("║    [C] CREATE ROOM    │ Create a new battle room                             ║\n");
            sendMessage.Append("║    [J] JOIN ROOM      │ Join an existing battle room                         ║\n");
            sendMessage.Append("║    [R] REFRESH        │ Refresh the room list                                ║\n");
            sendMessage.Append("║    [B] BACK           │ Return to main menu                                  ║\n");
            sendMessage.Append("║                                                                              ║\n");
            sendMessage.Append("╠══════════════════════════════════════════════════════════════════════════════╣\n");
            sendMessage.Append($"║ Wins: {user.Wins, -15} Losses: {user.Losses, -15} Win/Lose Ratio: {user.CalculateWinLossRatio(), -15}║\n");
            sendMessage.Append("╚══════════════════════════════════════════════════════════════════════════════╝\n");
            
            await session.SendMessageAsync(sendMessage.ToString());
            string choice = await session.GetInputAsync("\nChoice:");

            switch (choice.ToUpper())
            {
                case "C": // Create room
                    if (IsUserInAnyRoom(user))
                    {
                        await session.SendMessageAsync("You're already in a room. Please leave your current room first.");
                        continue;
                    }

                    string roomName;
                    do
                    {
                         roomName = await session.GetInputAsync("Enter a name for your battle room:");
                    } while (roomName.Length > 40);

                    if (string.IsNullOrWhiteSpace(roomName))
                        {
                            await session.SendMessageAsync("Room name cannot be empty.");
                            continue;
                        }
                    
                    var newRoom = new GameRoom
                    {
                        Name = roomName,
                        Host = user,
                        HostSession = session,
                        IsActive = true
                    };
                    
                    _activeRooms.TryAdd(newRoom.Id, newRoom);
                    user.InRoom = true;
                                        
                    StringBuilder waitingMenu = new StringBuilder();
                    waitingMenu.Append($@"
╔══════════════════════════════════════════════════════════════════════════════╗
║                                                                              ║
║                          WAITING FOR CHALLENGER                              ║
║                                                                              ║
║    Room: {newRoom.Name,-64}    ║
║                                                                              ║
║    Status: Waiting for an opponent...                                        ║
║                                                                              ║
╠══════════════════════════════════════════════════════════════════════════════╣
║                                                                              ║
║    [L] LEAVE ROOM     │ Cancel and close this battle room                    ║
║    DO NOT LEAVE THE ROOM VIA DISCONNECTING                                   ║
║                                                                              ║
╚══════════════════════════════════════════════════════════════════════════════╝");

                    await session.SendMessageAsync(waitingMenu.ToString());

                    await session.SendMessageAsync("\nChoice:");

                    bool waitingForChallenger = true;
                    while (waitingForChallenger)
                    {
                        // Check if anyone joined while we were waiting
                        if (newRoom.Challenger != null)
                        {
                            _roomsInBattle.TryAdd(newRoom.Id, true);
                            await Task.Delay(1000);

                            session.ClearPendingInput();
                            await Fight(newRoom);  
                            
                            _roomsInBattle.TryRemove(newRoom.Id, out _);
                            return;
                        }

                        try 
                        {
                            if (session.HasAvailableData())
                            {
                                string hostChoice = await session.GetInputAsync("");
                                
                                // Check for leave command without case sensitivity
                                if (!string.IsNullOrEmpty(hostChoice) && hostChoice.Trim().ToUpper() == "L")
                                {
                                    newRoom.IsActive = false;
                                    _activeRooms.TryRemove(newRoom.Id, out _);
                                    
                                    user.InRoom = false;
                                    
                                    using (var dbContext = new DatabaseContext())
                                    {
                                        user.InRoom = false;
                                        dbContext.Users.Update(user);
                                        await dbContext.SaveChangesAsync();
                                    }
                                    
                                    await session.SendMessageAsync("\nYou have left the room successfully.");
                                    
                                    waitingForChallenger = false;
                                    break; 
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[Room] Error handling leave command: {ex.Message}");
                        }
                        
                        // Add a reasonable pause between checks
                        await Task.Delay(500);
                    }

                    if (!waitingForChallenger)
                    {
                        continue; 
                    }
                    break;

                case "J": // Join room
                    if (IsUserInAnyRoom(user))
                    {
                        await session.SendMessageAsync("You're already in a room. Please leave your current room first.");
                        continue;
                    }
                    
                    if (availableRooms.Count == 0)
                    {
                        await session.SendMessageAsync("No rooms available to join.");
                        continue;
                    }
                    
                    string roomChoice = await session.GetInputAsync("Enter the room number to join:");
                    if (int.TryParse(roomChoice, out int roomIndex) && roomIndex > 0 && roomIndex <= availableRooms.Count)
                    {
                        var selectedRoom = availableRooms[roomIndex - 1];
                        
                        // Join the room
                        if (_activeRooms.TryGetValue(selectedRoom.Id, out var roomToJoin) && !roomToJoin.IsFull)
                        {
                            if (roomToJoin.HostSession == null)
                            {
                                await session.SendMessageAsync("The host is no longer available.");
                                continue;
                            }

                            roomToJoin.Challenger = user;
                            roomToJoin.ChallengerSession = session;
                            user.InRoom = true;
                            

                            while (user.InRoom)
                            {
                                await Task.Delay(500);
                                
                                // Check if the host disconnected
                                if (!NetworkMethods.IsUsernameActive(roomToJoin.Host!.Username!))
                                {
                                    user.InRoom = false;
                                    await session.SendMessageAsync("The host has disconnected. Returning to room selection.");
                                    break;
                                }
                                
                                // Check if battle has started
                                if (_roomsInBattle.ContainsKey(roomToJoin.Id))
                                {
                                    break; 
                                }
                            }
                        }
                        else
                        {
                            await session.SendMessageAsync("That room is no longer available.");
                        }
                    }
                    else
                    {
                        await session.SendMessageAsync("Invalid room number.");
                    }
                    break;

                case "R": // Refresh room list
                    continue;

                case "B": // Back
                    return;

                default:
                    await session.SendMessageAsync("Invalid option. Please try again.");
                    break;
            }
        }
    }
    
    private static bool IsUserInAnyRoom(User user)
    {
        return _activeRooms.Values.Any(r => 
            (r.Host?.Id == user.Id || r.Challenger?.Id == user.Id) && r.IsActive);
    }
    
    private static async Task Fight(GameRoom room)
    {
        try
        {
            _activeRooms.TryRemove(room.Id, out _);

            // Clear any pending input for both players multiple times to be extra thorough
            for (int i = 0; i < 3; i++)
            {
                if (room.HostSession != null) room.HostSession.ClearPendingInput();
                if (room.ChallengerSession != null) room.ChallengerSession.ClearPendingInput();
                await Task.Delay(50);
            }

            await Task.Delay(2000);

            // Create arena with explicit logging
            Console.WriteLine("[Battle] Creating arena object");
            var arena = new Arena.Arena(room.Host!, room.Challenger!, room.HostSession!, room.ChallengerSession!);

            await Task.Delay(2000);

            Console.WriteLine("[Battle] Calling StartBattle()");
            bool? winner = null;
            try
            {
                winner = await arena.StartBattle();
            } catch (Exception ex)
            {
                Console.WriteLine($"[Battle] Error during StartBattle: {ex.Message}");
                Console.WriteLine($"[Battle] Stack trace: {ex.StackTrace}");
                throw; // Rethrow to ensure cleanup happens
            }
            finally {
                _roomsInBattle.TryRemove(room.Id, out _);
            }

            using (var context = new DatabaseContext())
            {
                var dbHost = await context.Users.FindAsync(room.Host!.Id);
                var dbChallenger = await context.Users.FindAsync(room.Challenger!.Id);
                
                if (dbHost != null && dbChallenger != null)
                {
                    dbHost.Coins += 100;
                    dbChallenger.Coins += 100;
                    dbHost.InRoom = false;
                    dbChallenger.InRoom = false;
                    
                    if (winner == true)
                    {
                        dbHost.Wins += 1;
                        dbChallenger.Losses += 1;
                    }
                    else if (winner == false)
                    {
                        dbHost.Losses += 1;
                        dbChallenger.Wins += 1;
                    }
                    
                    var hostPokemon = context.PokemonMaster
                        .Where(p => p.OwnerId == room.Host.Id)
                        .ToList();

                    var challengerPokemon = context.PokemonMaster
                        .Where(p => p.OwnerId == room.Challenger.Id)
                        .ToList();

                    int originalHostCoins = dbHost.Coins;
                    int originalChallengerCoins = dbChallenger.Coins;

                    foreach (var pokemon in hostPokemon)
                    {
                        dbHost.Coins += pokemon.PayDay;
                        pokemon.PayDay = 0;
                    }

                    foreach (var pokemon in challengerPokemon)
                    {
                        dbChallenger.Coins += pokemon.PayDay;
                        pokemon.PayDay = 0;
                    }

                    await context.SaveChangesAsync();
                    
                }
            }

            await Task.Delay(2000); // Give players time to read the results
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Battle] Critical error in Fight method: {ex.Message}");
            Console.WriteLine($"[Battle] Stack trace: {ex.StackTrace}");

        }
        finally
        {
            // Always ensure rooms and player statuses are cleaned up
            if (room.Host != null)
            {
                room.Host.InRoom = false;
                using (var context = new DatabaseContext())
                {
                    var host = await context.Users.FindAsync(room.Host.Id);
                    if (host != null)
                    {
                        host.InRoom = false;
                        await context.SaveChangesAsync();
                    }
                }
            }

            if (room.Challenger != null)
            {
                room.Challenger.InRoom = false;
                using (var context = new DatabaseContext())
                {
                    var challenger = await context.Users.FindAsync(room.Challenger.Id);
                    if (challenger != null)
                    {
                        challenger.InRoom = false;
                        await context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}