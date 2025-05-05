using System.Net.Sockets;
using System.Text;
using PokemonPocket;

namespace Server
{
    public class ClientSession
    {
        private Socket _client;
        public string PlayerId { get; private set; }
        
        public ClientSession(Socket client, string playerId)
        {
            _client = client;
            PlayerId = playerId;
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
            // If got a prompt, send it first
            if (!string.IsNullOrEmpty(prompt)) { await SendMessageAsync(prompt); }

            var buffer = new byte[1_024];
            var received = await _client.ReceiveAsync(buffer, SocketFlags.None);
            
            return Encoding.UTF8.GetString(buffer, 0, received);
        }
        
        // For multiple choice questions
        public async Task<string> GetChoiceAsync(string prompt, params string[] options)
        {
            var fullPrompt = new StringBuilder(prompt);
            fullPrompt.AppendLine();
            
            for (int i = 0; i < options.Length; i++)
            {
                fullPrompt.AppendLine($"[{i+1}] {options[i]}");
            }
            
            fullPrompt.Append("Enter your choice: ");
            
            // Get input from client
            var response = await GetInputAsync(fullPrompt.ToString());
            return response.Trim();
        }
    }
}