using System;

namespace Server;

public class Shop
{
    public static async Task ShopMenu(ClientSession session)
    {
        await session.SendMessageAsync("Welcome to the Shop!");
        await session.SendMessageAsync("┌───────────────────────────────────┐\n│         Shop Menu                 │\n└───────────────────────────────────┘");
        await session.SendMessageAsync("[1] Buy Item");
        await session.SendMessageAsync("[2] Sell Item");
        await session.SendMessageAsync("[3] Exit");

        string choice = await session.GetInputAsync("Please enter your choice:");

        switch (choice)
        {
            case "1":
                // Buy item logic
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