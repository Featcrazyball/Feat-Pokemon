using System.Net.Sockets;
using Database;
using System.Threading;

namespace Server
{
    class Server
    {
        static async Task Main() {
            // For submission
            // Initialize database before starting server
            using (var context = new DatabaseContext()) {
                context.Database.EnsureCreated();
                Console.WriteLine("Database initialized.");
            }

            // For testing only. plz delete after
            // Console.WriteLine("Initializing database...");
            // try
            // {
            //     // Check if the database file exists
            //     string dbPath = "database.db";
            //     if (File.Exists(dbPath))
            //     {
            //         File.Delete(dbPath);
            //         Console.WriteLine("Deleted existing database file");
            //     }

            //     if (File.Exists(dbPath + "-wal"))
            //         File.Delete(dbPath + "-wal");
            //     if (File.Exists(dbPath + "-shm"))
            //         File.Delete(dbPath + "-shm");

            //     // Create fresh database
            //     using var context = new DatabaseContext();
            //     context.Database.EnsureCreated();
            //     Console.WriteLine("Created new database");
            // }
            // catch (Exception ex)
            // {
            //     Console.WriteLine($"Database initialization error: {ex.Message}");
            // }

            // Reset Arena Logins + upgrade any plaintext passwords to hashes
            using (var context = new DatabaseContext())
            {
                var users = context.Users.ToList();
                int upgraded = 0;
                foreach (var user in users)
                {
                    user.InRoom = false;
                    if (!string.IsNullOrEmpty(user.Password) && !PasswordHasher.IsHashed(user.Password))
                    {
                        user.Password = PasswordHasher.Hash(user.Password);
                        upgraded++;
                    }
                    context.Users.Update(user);
                }
                context.SaveChanges();
                if (upgraded > 0)
                    Console.WriteLine($"Hashed {upgraded} plaintext password(s).");
            }

            // Seed a default admin account if there are no users
            using (var context = new DatabaseContext())
            {
                if (!context.Users.Any())
                {
                    const string defaultUsername = "admin";
                    const string defaultPassword = "admin1234";
                    var admin = new Models.User(defaultUsername, defaultPassword, "admin@localhost.com")
                    {
                        God = true,
                        Coins = 1000000
                    };
                    context.Users.Add(admin);
                    context.SaveChanges();
                    Console.WriteLine($"No users found. Created default account -> username: {defaultUsername} | password: {defaultPassword}");
                    Console.WriteLine("Change this password from Settings after logging in.");
                }
            }

            // Server Setup
            Socket server;
            try            
            {
                server = await NetworkMethods.ServerSetup();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting up server: {ex.Message}");
                return;
            }

            _ = Task.Run(async () =>
            {
                while (true)
                {
                    NetworkMethods.ServerCheckClientConnections();
                    await Task.Delay(1000); 
                }
            });

            // Main client acceptance loop
            while (true)
            {
                try
                {
                    var client = await server.AcceptAsync();
                    
                    // Handle the client in a separate task without waiting
                    _ = Task.Run(() => NetworkMethods.HandleClient(client));
                }
                catch (SocketException ex)
                {
                    Console.WriteLine($"Socket exception: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"General exception: {ex.Message}");
                }
            }
        }
    }
}