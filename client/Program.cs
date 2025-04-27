using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Client
    {
        public static async Task Main() 
        {
            IPAddress ip = IPAddress.Parse("39.109.136.104");
            IPEndPoint iPEndPoint = new(ip, 8000);

            using Socket client = new(
                iPEndPoint.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp
            );

            await client.ConnectAsync(iPEndPoint);

            while (true) {
                var message = Console.ReadLine();

                var messageBytes = Encoding.UTF8.GetBytes(message ?? string.Empty);
                await client.SendAsync(messageBytes, SocketFlags.None);

                var buffer = new byte[1_024];

                var received = await client.ReceiveAsync(buffer, SocketFlags.None);

                var messageString = Encoding.UTF8.GetString(buffer, 0, received);

                Console.WriteLine(messageString);
            }
        }
    }
}