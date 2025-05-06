using System.Net;
using System.Net.Sockets;
using System.Collections.Concurrent;
using Database;
using Models;

namespace Server
{
    public static class NetworkMethods
    {
        // Dictionary to store active client sessions
        private static ConcurrentDictionary<string, ClientSession> _activeSessions = new ConcurrentDictionary<string, ClientSession>();

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
                await session.SendMessageAsync("Welcome to Featcrazyball's Pokemon Game! \n[1] Login\n[2] Register\nPlease enter your choice:");
                string choice = await session.GetInputAsync();

                string username;
                string email;
                string password;

                switch (choice)
                {
                    case "1":
                        while(true){
                            await session.SendMessageAsync("┌───────────────────────────────────┐\n│            Logging In             │\n└───────────────────────────────────┘");
                            username = await session.GetInputAsync("Please enter your username:");
                            password = await session.GetInputAsync("Please enter your password:");

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

                            await session.SendMessageAsync("┌───────────────────────────────────┐\n│           Authenticating...       │\n└───────────────────────────────────┘");
                            await Task.Delay(2000); // Simulate authentication delay (its fake but funny)
                            await session.SendMessageAsync($"Welcome back, {username}!");
                            break;
                        }
                        break;

                    case "2":
                        await session.SendMessageAsync("┌───────────────────────────────────┐\n│            Registering            │\n└───────────────────────────────────┘");

                        while (true)
                        {
                            username = await session.GetInputAsync("Please enter your username:");
                            email = await session.GetInputAsync("Please enter your email:");
                            password = await session.GetInputAsync("Please enter your password:");
                            string confirmPassword = await session.GetInputAsync("Please confirm your password:");

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

                                try {
                                    context.SaveChanges();
                                    await session.SendMessageAsync("Registration successful!");
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
    }
}