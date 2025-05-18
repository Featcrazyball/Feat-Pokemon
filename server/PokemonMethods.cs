using Database;
using Models;
namespace Server;

public static class ExtraMethods
{
    public static string CenterAlign(string text, int width)
    {
        if (string.IsNullOrEmpty(text))
            return new string(' ', width);
            
        int padding = width - text.Length;
        int padLeft = padding / 2 + text.Length;
        return text.PadLeft(padLeft).PadRight(width);
    }
}