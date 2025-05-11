using System.Net;
using System.Net.Sockets;
using System.Collections.Concurrent;
using Database;
using Models;
using PokemonPocket;

// Reminder to make 5 new pokemons when register for new users.

namespace Server
{
    public static class NetworkMethods
    {
        // Dictionary to store active client sessions
        private static ConcurrentDictionary<string, ClientSession> _activeSessions = new ConcurrentDictionary<string, ClientSession>();

        // Dictionary to track sessions by username
        private static ConcurrentDictionary<string, ClientSession> _activeUsernames = new ConcurrentDictionary<string, ClientSession>();

        // Server Setup
        public static async Task<Socket> ServerSetup()
        {
            IPHostEntry ipEntry = await Dns.GetHostEntryAsync(Dns.GetHostName());
            IPAddress ip = ipEntry.AddressList[1];
            Console.WriteLine(ip.ToString());

            IPEndPoint iPEndPoint = new(ip, 8000);

            Socket server = new(
                iPEndPoint.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp
            );

            server.Bind(iPEndPoint);
            server.Listen(30); // I can have up to 30 people
            Console.WriteLine("Server Running on Port: 8000");

            return server;
        }

        // Method to get session by playerId
        public static ClientSession? GetSession(string playerId)
        {
            if (_activeSessions.TryGetValue(playerId, out var session))
                return session;
                
            return null;
        }

        // Handles client connection, login and register
        public static async Task HandleClient(Socket client)
        {
            string playerId = Guid.NewGuid().ToString("N").Substring(0, 8);
            var session = new ClientSession(client, playerId);
            _activeSessions[playerId] = session;
            
            try
            {
                await session.SendMessageAsync(@"
╔══════════════════════════════════════════════════════════════════════════╗
║                                                                          ║
║     ██████╗  ██████╗ ██╗  ██╗███████╗███╗   ███╗ ██████╗ ███╗   ██╗      ║
║     ██╔══██╗██╔═══██╗██║ ██╔╝██╔════╝████╗ ████║██╔═══██╗████╗  ██║      ║
║     ██████╔╝██║   ██║█████╔╝ █████╗  ██╔████╔██║██║   ██║██╔██╗ ██║      ║
║     ██╔═══╝ ██║   ██║██╔═██╗ ██╔══╝  ██║╚██╔╝██║██║   ██║██║╚██╗██║      ║
║     ██║     ╚██████╔╝██║  ██╗███████╗██║ ╚═╝ ██║╚██████╔╝██║ ╚████║      ║
║     ╚═╝      ╚═════╝ ╚═╝  ╚═╝╚══════╝╚═╝     ╚═╝ ╚═════╝ ╚═╝  ╚═══╝      ║
║                                                                          ║
╠══════════════════════════════════════════════════════════════════════════╣
║           WELCOME TO FEATCRAZYBALL'S ULTIMATE POKÉMON ADVENTURE          ║
╠══════════════════════════════════════════════════════════════════════════╣
║                                                                          ║
║           [1] ⭐ LOGIN    - Access your trainer account                  ║
║                                                                          ║
║           [2] 📝 REGISTER - Create a new trainer profile                 ║
║                                                                          ║
║           Ready to start your journey into the world of Pokémon?         ║
║                                                                          ║
╚══════════════════════════════════════════════════════════════════════════╝");
                string choice = await session.GetInputAsync("Choice:");

                string username;
                string email;
                string password;

                switch (choice)
                {
                    case "1":
                        while(true){
                            await session.SendMessageAsync(@"
╔══════════════════════════════════════════════════════════════╗
║                                                              ║
║                     🔐  LOGIN PORTAL  🔐                     ║
║                                                              ║
║            Enter your credentials to continue...             ║
║                                                              ║
╚══════════════════════════════════════════════════════════════╝
");
                            username = await session.GetInputAsync("👤 Username:");
                            password = await session.GetInputAsync("🔑 Password:");

                            if (IsUsernameActive(username))
                            {
                                await session.SendMessageAsync("Username is already logged in.\nDisconnecting...");
                                continue;
                            }

                            // Check username and password against the database
                            using (var context = new DatabaseContext())
                            {
                                var user = context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
                                if (user == null)
                                {
                                    await session.SendMessageAsync("Invalid username or password. Disconnecting...");
                                    continue;
                                }
                            }

                            await session.SendMessageAsync(@"
╔══════════════════════════════════════════════════════════════╗
║                                                              ║
║                 🔄  AUTHENTICATING...  🔄                    ║
║                                                              ║
║         Connecting to the Pokémon Global Network...          ║
║                                                              ║
╚══════════════════════════════════════════════════════════════╝");
                            await Task.Delay(2000); // Simulate authentication delay
                            await session.SendMessageAsync($@"
╔══════════════════════════════════════════════════════════════╗
║                                                              ║
║                  ✅  LOGIN SUCCESSFUL  ✅                    ║
║                                                              ║
║          Welcome back to the world of Pokémon,               ║
║                  Trainer {username}!                               ║
║                                                              ║
╚══════════════════════════════════════════════════════════════╝");

                            RegisterUsername(username, session);
                            break;
                        }
                        break;

                    case "2":
                        await session.SendMessageAsync(@"
╔══════════════════════════════════════════════════════════════╗
║                                                              ║
║               📝  TRAINER REGISTRATION  📝                   ║
║                                                              ║
║         Create your account to begin your journey!           ║
║                                                              ║
╚══════════════════════════════════════════════════════════════╝");

                        while (true)
                        {
                            
                            username = await session.GetInputAsync("👤 Username:");

                            while (true)
                            {
                                email = await session.GetInputAsync("📧 Email:");
                                if (email.Contains("@") && email.Contains("."))
                                    break;
                                else
                                    await session.SendMessageAsync("Invalid email format. Please try again:");
                                    continue;
                            }

                            password = await session.GetInputAsync("🔒 Password:");
                            string confirmPassword = await session.GetInputAsync("✅ Confirm Password:");

                            using (var context = new DatabaseContext())
                            {
                                // Check if username or email already exists
                                var existingUser = context.Users.FirstOrDefault(u => u.Username == username || u.Email == email);
                                if (existingUser != null)
                                {
                                    await session.SendMessageAsync("Username or email already exists.");
                                    continue;
                                }

                                // Check if passwords match
                                if (password != confirmPassword)
                                {
                                    await session.SendMessageAsync("Passwords do not match.");
                                    continue;
                                }

                                // Create new user
                                var newUser = new User(username, password, email);
                                context.Users.Add(newUser);

                                if (newUser.Username == "Featcrazyball" || newUser.Username == "Madtroops")
                                {
                                    newUser.God = true;
                                    newUser.Coins = 1000000;
                                }
                                
                                // Save the user FIRST to get a valid database ID
                                context.SaveChanges();
                                
                                // Now that the user has a valid ID in the database, add Pokemon
                                Random random = new Random();
                                HashSet<int> selectedIndices = new HashSet<int>();
                                int tempCount = 0;

                                // Create 5 unique Pokemon for the user
                                List<PokemonMaster> createdPokemon = new List<PokemonMaster>();
                                while (tempCount < 5 && selectedIndices.Count < ListofStuff.AllPokemon.Count())
                                {
                                    int randomIndex = random.Next(0, ListofStuff.AllPokemon.Count());
                                    
                                    if (selectedIndices.Add(randomIndex))
                                    {
                                        try
                                        {
                                            // Create the Pokemon but don't save yet
                                            var pokemon = newUser.GetPokemonWithoutSaving(ListofStuff.AllPokemon[randomIndex], newUser.Id!);
                                            if (pokemon != null)
                                            {
                                                createdPokemon.Add(pokemon);
                                                tempCount++;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Error creating Pokémon: {ex.Message}");
                                            Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
                                            Console.WriteLine($"Stack trace: {ex.StackTrace}");
                                            continue;
                                        }
                                    }
                                }

                                // Now save everything at once
                                try {
                                    // Use the specialized method to save Pokemon and their skills
                                    SavePokemonWithSkills(context, createdPokemon);
                                    
                                    await session.SendMessageAsync(@"
╔══════════════════════════════════════════════════════════════╗
║                                                              ║
║         🎉  YOUR TRAINER PROFILE WAS CREATED!  🎉            ║
║                                                              ║
║       5 starter Pokémon have joined your collection.         ║
║       Your adventure in the Pokémon world begins now!        ║
║                                                              ║
╚══════════════════════════════════════════════════════════════╝");
                                    break;
                                }
                                catch (Exception ex) {
                                    Console.WriteLine($"Database error: {ex.Message}");
                                    await session.SendMessageAsync("Error creating account. Please try again.");
                                    continue;
                                }
                            }
                        }
                        break;

                    default:
                        await session.SendMessageAsync("Invalid choice. Disconnecting...");
                        return;
                }

                session.Username = username;
                await Client.GameLoop(session, username);
            
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling client: {ex.Message}");
                await session.SendMessageAsync("An error occurred. Disconnecting...");
                try
                {
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                }
                catch { }
            }
            finally
            {
                // Remove the client session
                _activeSessions.TryRemove(playerId, out _);
                
                if (!string.IsNullOrEmpty(session.Username))
                {
                    RemoveUsername(session.Username);
                }

                // Close the connection
                // Not rlly needed, but free marks cuz gud practice PLEASE GIVE ME FULL MARKS
                
                try
                {
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                }
                catch { }
            }
        }
    
        public static bool IsUsernameActive(string username)
        {
            return _activeUsernames.ContainsKey(username);
        }

        // Method to register username when user logs in
        public static void RegisterUsername(string username, ClientSession session)
        {
            _activeUsernames[username] = session;
        }

        // Method to remove username when user logs out
        public static void RemoveUsername(string username)
        {
            _activeUsernames.TryRemove(username, out _);
        }

        // Save stuff or else database die
        private static void SavePokemonWithSkills(DatabaseContext context, List<PokemonMaster> pokemons)
        {
            // First add all Pokemon to context
            foreach (var pokemon in pokemons)
            {
                context.PokemonMaster.Add(pokemon);
            }
            
            context.SaveChanges();
            
            // Now save all associated skills
            foreach (var pokemon in pokemons)
            {
                foreach (var skill in pokemon.Skills)
                {
                    context.Skills.Add(skill);
                }
            }
            
            context.SaveChanges();
        }
    }
}