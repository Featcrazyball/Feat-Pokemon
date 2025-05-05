using System.Net.Sockets;
using Database;
using System.Threading;

namespace Server
{
    class Server
    {
        static async Task Main() {
            // Initialize database before starting server
            using (var context = new DatabaseContext()) {
                context.Database.EnsureCreated();
                Console.WriteLine("Database initialized.");
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
                var client = await server.AcceptAsync();
                var customer = Task.Run(() => NetworkMethods.HandleClient(client));
            } 
        }
    }
}