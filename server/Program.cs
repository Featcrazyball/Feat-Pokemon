using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using PokemonPocket;

namespace Server
{
    class Server
    {
        static async Task Main() {
            // Setup Skill Pool
            StartupMethods.SetUpSkillPool();

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