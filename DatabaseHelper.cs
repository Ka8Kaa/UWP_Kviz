using System.Data.SQLite;
using System.IO;

public static class DatabaseHelper
{
    private static string connectionString= @"DataSource=Databaza.db;Version=3";
    public static void InitializeDatabase()
    {
        if(!File.Exists(connectionString))
        {
            SQLiteConnection.CreateFile(connectionString);
            using (var connection=new SQLiteConnection(connectionString))
            {
                connection.Open();
                string createPitanjaTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Pitanja(
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    QuestionText TEXT NOT NULL,
                    CorrectAnswer TEXT NOT NULL,
                    WrongAnswer1 TEXT NOT NULL,
                    WrongAnswer2 TEXT NOT NULL,
                    WrongAnswer3 TEXT NOT NULL
                  );";
                string createOsobniPodaciTableQuery = @"
                   CREATE TABLE IF NOT EXISTS OsobniPodaci(
                    ID_Unos  INTEGER PRIMARY KEY AUTOINCREMENT,
                    OIB BIGINT NOT NULL,
	                Ime TEXT NOT NULL,
	                Prezime TEXT NOT NULL
                  );";

                using (var command=new SQLiteCommand(connection))
                {
                    command.CommandText= createPitanjaTableQuery;
                    command.ExecuteNonQuery();

                    command.CommandText= createOsobniPodaciTableQuery;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
