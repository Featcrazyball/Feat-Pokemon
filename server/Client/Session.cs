using System.Net.Sockets;
using System.Text;
using PokemonPocket;

namespace Server
{
    public class ClientSession
    {
        public Socket _client;  // Keep this private for encapsulation
        public string PlayerId { get; private set; }
        public string? Username { get; set; }
        public bool InChat { get; set; } = false;
        
        // All Clients
        public static readonly List<ClientSession> _allClients = new List<ClientSession>();

        public ClientSession(Socket client, string playerId)
        {
            _client = client;
            PlayerId = playerId;

            lock (_allClients)
            {
                _allClients.Add(this);
            }
        }
        
        public void ClearPendingInput()
        {
            try
            {
                if (_client.Available > 0)
                {
                    byte[] buffer = new byte[_client.Available];
                    _client.Receive(buffer, SocketFlags.None);
                    Console.WriteLine($"[Session] Cleared {buffer.Length} bytes of pending input");
                }
            }
            catch (Exception ex)
            {
                // Just log the exception without throwing
                Console.WriteLine($"[Session] Error clearing pending input: {ex.Message}");
            }
        }

        // Add a method to check if input is available without blocking
        public bool HasPendingInput()
        {
            try 
            {
                return _client.Available > 0;
            }
            catch 
            {
                return false;
            }
        }

        // Add a method to check if data is available in the socket
        public bool HasAvailableData()
        {
            try
            {
                return _client != null && _client.Connected && _client.Available > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Session] Error checking available data: {ex.Message}");
                return false;
            }
        }
        
        // Add a method to get the number of available bytes
        public int AvailableBytes()
        {
            try
            {
                return _client?.Available ?? 0;
            }
            catch
            {
                return 0;
            }
        }


        // Replace Console.WriteLine - sends messages to the client
        public async Task SendMessageAsync(string message)
        {
            try
            {
                var messageBytes = Encoding.UTF8.GetBytes(message);
                await _client.SendAsync(messageBytes, SocketFlags.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message to client: {ex.Message}");
            }
        }
        
        // Replace Console.ReadLine - gets input from the client
        public async Task<string> GetInputAsync(string? prompt = null)
        {
            try 
            {
                // If got a prompt, send it first
                if (!string.IsNullOrEmpty(prompt)) 
                { 
                    await SendMessageAsync(prompt);
                    Console.WriteLine($"[Session] Sent prompt: {prompt}");
                }

                Console.WriteLine("[Session] Waiting for client input...");
                var buffer = new byte[1_024];
                int received;
                
                try {
                    received = await _client.ReceiveAsync(buffer, SocketFlags.None);
                    Console.WriteLine($"[Session] Received {received} bytes of input");
                }
                catch (Exception ex) {
                    Console.WriteLine($"[Session] Error receiving input: {ex.Message}");
                    return "Error receiving input";
                }
                
                var response = Encoding.UTF8.GetString(buffer, 0, received);
                Console.WriteLine($"[Session] Parsed response: {response}");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Session] Error in GetInputAsync: {ex.Message}");
                return "Error";
            }
        }
        
        // Get all clients connected to the server
        public static List<ClientSession> GetAllClients()
        {
            return _allClients.ToList();
        }
    }
}