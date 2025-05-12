using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Client
    {
        private static bool _isConnected = false;
        private static readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public static async Task Main() 
        {
            try
            {
                // Connect to server
                Console.WriteLine("Connecting to server...");
                IPAddress ip = IPAddress.Parse("39.109.136.104");
                IPEndPoint iPEndPoint = new(ip, 8000);

                using Socket client = new(
                    iPEndPoint.AddressFamily,
                    SocketType.Stream,
                    ProtocolType.Tcp
                );

                // Exception handling for connection
                try
                {
                    await client.ConnectAsync(iPEndPoint);
                    _isConnected = true;
                    Console.WriteLine("Connected to server!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to connect: {ex.Message}");
                    return;
                }

                // Start receiving messages in a separate thread
                Task receiveTask = ReceiveMessagesAsync(client);

                // Handle user input in the main thread
                while (_isConnected)
                {
                    try
                    {
                        string input = Console.ReadLine() ?? string.Empty;
                        if (input.ToLower() == "exit")
                        {
                            _cts.Cancel();
                            break;
                        }

                        if (_isConnected)
                        {
                            await SendMessageAsync(client, input);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error sending message: {ex.Message}");
                        break;
                    }
                }

                // Wait for the receive task to complete
                try
                {
                    await receiveTask;
                }
                catch (OperationCanceledException) { Console.WriteLine("Received Task Cancelled."); }
            }
            finally
            {
                Console.WriteLine("Disconnected from server.");
                _isConnected = false;
                _cts.Cancel();
                _cts.Dispose();
                Environment.Exit(0);
            }
        }

        // Receive messages from the server asynchronously
        private static async Task ReceiveMessagesAsync(Socket client)
        {
            byte[] buffer = new byte[8192];
            
            while (!_cts.Token.IsCancellationRequested && _isConnected)
            {
                try
                {
                    int received = await client.ReceiveAsync(buffer, SocketFlags.None, _cts.Token);
                    if (received == 0)
                    {
                        _isConnected = false;
                        Console.WriteLine("\nServer closed the connection.");
                        break;
                    }

                    string message = Encoding.UTF8.GetString(buffer, 0, received);
                    
                    if (message.Contains("2q30-8b6r7-vyq20974ryf-b09qw8r7bq9-28-3v")) {Environment.Exit(0);} 

                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine(message);
                    Console.ResetColor();
                    if (IsMessage(message)) { Console.Write("> "); }
                }
                catch (SocketException)
                {
                    _isConnected = false;
                    Console.WriteLine("\nConnection to server lost.");
                    break;
                }
                catch (OperationCanceledException)
                {
                    _isConnected = false;
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError receiving message: {ex.Message}");
                    _isConnected = false;
                    break;
                }
            }
        }

        // Send messages to the server asynchronously
        private static async Task SendMessageAsync(Socket client, string message)
        {
            try
            {
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                await client.SendAsync(messageBytes, SocketFlags.None);
            }
            catch (Exception)
            {
                _isConnected = false;
                throw;
            }
        }
    
        // Bad but I have no choice...
        private static bool IsMessage(string m)
        {
            string[] s = new[]
            {
                "👤 Username  : ",
                "📧 Email     : ",
                "🔒 Password  : ",
                "✅ Confirm   : ",
                "👤 Username:",
                "🔑 Password:",
                "Input any key to continue...",
                "Enter the number of the Pokémon to nickname:",
                "Enter option or Pokémon number:",
                "Enter a new nickname for",
                "Please enter your choice:",
                "Enter the stat to allocate points to (HP, ATK, DEF, SpA, SpD, SPD) or 'done' to finish:",
                "Enter the number of points to allocate:",
                "Enter the number of the"
            };

            foreach (var str in s) { if (m.Contains(str)) { return true; }}  

            return false;
        }

    }
}