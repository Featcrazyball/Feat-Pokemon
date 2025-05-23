using System;
using System.Text;
using Models;
using Database;
using PokemonPocket;
using Server;

namespace Server;

public class ShopInventory
{
    public static async Task InventoryMenu(ClientSession session)
    {
        using var context = new DatabaseContext();
        User user = context.Users.FirstOrDefault(u => u.Username == session.Username)!;
        if (user == null)
        {
            await session.SendMessageAsync("There has been an error locating your account. Please try again.");
            return;
        }
        while (true)
        {
            var sendMessage = new StringBuilder();
            sendMessage.Append(@"
╔══════════════════════════════════════════════════════════════════════════════╗
║                                                                              ║
║      ✨✨✨✨✨✨✨✨✨    INVENTORY MENU    ✨✨✨✨✨✨✨✨✨              ║
║                                                                              ║
╠══════════════════════════════════════════════════════════════════════════════╣
║                                                                              ║");

            // Moon Stones
            var moonStones = context.Items.Where(i => i.Name == "Moon Stone" && i.OwnerId == user.Id).ToList();
            int moonStoneCount = moonStones.Count;

            // Fire Stones
            var fireStones = context.Items.Where(i => i.Name == "Fire Stone" && i.OwnerId == user.Id).ToList();
            int fireStoneCount = fireStones.Count;

            // Water Stones
            var waterStones = context.Items.Where(i => i.Name == "Water Stone" && i.OwnerId == user.Id).ToList();
            int waterStoneCount = waterStones.Count;

            // Thunder Stones
            var thunderStones = context.Items.Where(i => i.Name == "Thunder Stone" && i.OwnerId == user.Id).ToList();
            int thunderStoneCount = thunderStones.Count;

            // Leaf Stones
            var leafStones = context.Items.Where(i => i.Name == "Leaf Stone" && i.OwnerId == user.Id).ToList();
            int leafStoneCount = leafStones.Count;

            // Coins
            int coins = user.Coins;

            if (moonStoneCount > 0)
            {
                sendMessage.Append($"\n║    🌙 MOON STONE  │ {moonStoneCount} Moon Stone(s) in your inventory         ║");
                sendMessage.Append($"\n║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━     ║");
            }

            if (fireStoneCount > 0)
            {
                sendMessage.Append($"\n║    🔥 FIRE STONE  │ {fireStoneCount} Fire Stone(s) in your inventory         ║");
                sendMessage.Append($"\n║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━     ║");
            }

            if (waterStoneCount > 0)
            {
                sendMessage.Append($"\n║    💧 WATER STONE │ {waterStoneCount} Water Stone(s) in your inventory       ║");
                sendMessage.Append($"\n║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━     ║");
            }

            if (thunderStoneCount > 0)
            {
                sendMessage.Append($"\n║    ⚡ THUNDER STONE│ {thunderStoneCount} Thunder Stone(s) in your inventory  ║");
                sendMessage.Append($"\n║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━     ║");
            }

            if (leafStoneCount > 0)
            {
                sendMessage.Append($"\n║    🍃 LEAF STONE  │ {leafStoneCount} Leaf Stone(s) in your inventory         ║");
                sendMessage.Append($"\n║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━     ║");
            }

            sendMessage.Append($"\n║     COINS: {user.Coins, -9}                                                         ║");
            sendMessage.Append("\n║                                                                              ║");
            sendMessage.Append("\n╚══════════════════════════════════════════════════════════════════════════════╝");

            Console.WriteLine(user.Coins + " coins");

            await session.SendMessageAsync(sendMessage.ToString());
            await session.GetInputAsync("\nInput any key to continue...");
            break;
        }
    }
}