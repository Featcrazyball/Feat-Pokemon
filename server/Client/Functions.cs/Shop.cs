using System;
using System.Text;
using Models;

namespace Server;

public class Shop
{
    public static async Task ShopMenu(ClientSession session)
    {
        await session.SendMessageAsync(@"
╔══════════════════════════════════════════════════════════════════════════════╗
║                                                                              ║
║      ✨✨✨✨✨✨✨✨✨    SHOP MENU    ✨✨✨✨✨✨✨✨✨                  ║    
║                                                                              ║
╠══════════════════════════════════════════════════════════════════════════════╣
║                                                                              ║
║    [1] 🔥 SHOP     │ Buy Items from Shop                                     ║
║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║
║    [2] 💰 IVENTORY │ View Your Current Inventory                             ║
║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║
║    [3] 💬 BACK     │ Return to Trainer Menu                                  ║
║    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━              ║
║                                                                              ║
╚══════════════════════════════════════════════════════════════════════════════╝
");

        string choice = await session.GetInputAsync("Please enter your choice:");

        switch (choice)
        {
            case "1":
                await ShopBuy.BuyItem(session);
                break;
            case "2":
                // Sell item logic
                break;
            case "3":
                // Exit shop
                break;
            default:
                await session.SendMessageAsync("Invalid choice. Please try again.");
                break;
        }
    }
}