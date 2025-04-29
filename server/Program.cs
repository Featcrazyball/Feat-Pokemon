using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Server
{
    class Server
    {
        static async Task Main() {
            // Initialise Database
            using SqliteConnection db = new("Data Source=database.db");
            db.Open();

            // Server Setup
            Socket server;
            try            
            {
                server = await ServerSetup();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting up server: {ex.Message}");
                return;
            }

            // Client Loop
            while (true) {
                var client = await server.AcceptAsync();
                var customer = Task.Run(() => HandleClient(client));
            } 
        }

        // Server Setup
        public static async Task<Socket> ServerSetup()
        {
            IPHostEntry ipEntry = await Dns.GetHostEntryAsync(Dns.GetHostName());
            IPAddress ip = ipEntry.AddressList[1];
            Console.WriteLine(ip.ToString());

            IPEndPoint iPEndPoint = new(ip, 8000);

            Socket server = new (
                iPEndPoint.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp
            );

            server.Bind(iPEndPoint);
            server.Listen(30); // I can have up to 30 people
            Console.WriteLine("Server Running on Port: 8000");

            return server;
        }

        // Actual Game Loop
        static async Task HandleClient(Socket client)
        {
            try
            {
                while (true) {
                    var buffer = new byte[1_024];
                    var received = await client.ReceiveAsync(buffer, SocketFlags.None);
                    
                    // Check if got data (if theres no data means it disconnected)
                    if (received == 0)
                    {
                        Console.WriteLine("Client disconnected or no data received.");
                        break; 
                    }
                    
                    var messageString = Encoding.UTF8.GetString(buffer, 0, received);

                    // Check and close if it's HTTP request
                    if (messageString.StartsWith("GET ") || messageString.StartsWith("POST ")) {
                        try {
                            var forbiddenResponse = "HTTP/1.1 403 Forbidden\r\nContent-Type: text/plain\r\n\r\n403 Forbidden";
                            var responseBytes = Encoding.UTF8.GetBytes(forbiddenResponse);
                            await client.SendAsync(responseBytes, SocketFlags.None);
                        } catch (SocketException ex) {
                            Console.WriteLine($"SocketException caught: {ex.Message}");
                            break;
                        } 
                    } else if (messageString != null) {
                        // Dont remove, if remove and someone say hello, weird things happen
                        if (messageString == "Hello") {
                            Console.WriteLine($"Message from client: Hello");
                        }
                        Console.WriteLine($"Message from client: {messageString}");
                        var response = "Message Received";
                        var responseByte = Encoding.UTF8.GetBytes(response);
                        await client.SendAsync(responseByte, SocketFlags.None);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling client: {ex.Message}");
            }
            finally
            {
                try
                {
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                }
                catch (Exception err)
                {
                    Console.WriteLine($"Error Cutting Off Connection: {err.Message}");
                }
            }
        }
    }
}