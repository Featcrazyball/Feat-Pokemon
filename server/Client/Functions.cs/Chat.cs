using Server;

namespace Server;

public class Chat
{
    public static async Task ChatMenu(ClientSession session)
    {
        await session.SendMessageAsync(@"
╔══════════════════════════════════════════════════════════════════════════════╗
║                                                                              ║
║      ✨✨✨✨✨✨✨✨✨       CHAT     ✨✨✨✨✨✨✨✨✨                    ║
║                                                                              ║
║          Welcome to the Chat Area! Type 'exit' to leave the chat.            ║
║       You can chat with other trainers here. Type your message below:        ║
║                                                                              ║
╚══════════════════════════════════════════════════════════════════════════════╝
");
        session.InChat = true;
        while (true)
        {
            string message = await session.GetInputAsync("");
            message = message.Trim();
            if (message.ToLower() == "exit")
            {
                session.InChat = false;
                return;
            }

            if (!NetworkMethods.IsUsernameActive(session.Username!))
            {
                await session.SendMessageAsync("The host has disconnected. Returning to room selection.");
                break;
            }

            // list of all clients
            var clients = ClientSession.GetAllClients();

            // Broadcast the message to all connected clients
            foreach (var client in clients)
            {
                if (client != session && client.InChat)
                {
                    await client.SendMessageAsync($"{session.Username}: {message}");
                }
            }
        }
    }
}