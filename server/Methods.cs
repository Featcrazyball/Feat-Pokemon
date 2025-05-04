using Database;
namespace Server;

public static class StartupMethods
{
    public static void SetUpSkillPool()
    {
        using var db = new DatabaseContext();
        db.Database.EnsureCreated(); // Create the database if it doesn't exist

    }
}