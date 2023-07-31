using System.Data.SQLite;

namespace FaradayBot
{
    public class DatabaseExtractor
    {
        public static string? GetLatestFieldValue(string databasePath, string tableName, string fieldName, string orderByField)
        {
            string connectionString = $"Data Source={databasePath};Version=3;";
            string? fieldValue = null;

            using (SQLiteConnection connection = new(connectionString))
            {
                connection.Open();

                string query = $"SELECT {fieldName} FROM {tableName} ORDER BY {orderByField} DESC LIMIT 1;";

                using (SQLiteCommand command = new(query, connection))
                {
                    using SQLiteDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        fieldValue = reader[fieldName].ToString();
                    }
                }

                connection.Close();
            }

            return fieldValue;
        }
    }
}
