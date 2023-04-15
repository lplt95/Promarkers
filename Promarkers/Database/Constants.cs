namespace Promarkers.Database;

public static class Constants
{
    public const string DatabaseFilename = "PromarkersBase.db";

    public const SQLite.SQLiteOpenFlags Flags = SQLite.SQLiteOpenFlags.ReadWrite | SQLite.SQLiteOpenFlags.Create | SQLite.SQLiteOpenFlags.SharedCache;

    public static string DatabasePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DatabaseFilename);
    
}
