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
            // using (var context = new DatabaseContext()) {
            //     context.Database.EnsureCreated();
            //     Console.WriteLine("Database initialized.");
            // }

            // For testing only. plz delete after
            Console.WriteLine("Initializing database...");
            try
            {
                // Check if the database file exists
                string dbPath = "database.db";
                if (File.Exists(dbPath))
                {
                    File.Delete(dbPath);
                    Console.WriteLine("Deleted existing database file");
                }

                if (File.Exists(dbPath + "-wal"))
                    File.Delete(dbPath + "-wal");
                if (File.Exists(dbPath + "-shm"))
                    File.Delete(dbPath + "-shm");

                // Create fresh database
                using var context = new DatabaseContext();
                context.Database.EnsureCreated();
                Console.WriteLine("Created new database");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database initialization error: {ex.Message}");
            }

            // Reset Arena Logins
            using (var context = new DatabaseContext())
            {
                var users = context.Users.ToList();
                foreach (var user in users)
                {
                    user.InRoom = false;
                    context.Users.Update(user);
                }
                context.SaveChanges();
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

            // Client Loop
            while (true) {
                try
                {
                    var client = await server.AcceptAsync();
                    var customer = Task.Run(() => NetworkMethods.HandleClient(client));
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