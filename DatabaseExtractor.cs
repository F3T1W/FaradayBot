using System.Data.SQLite;

namespace FaradayBot
{
    public class DatabaseExtractor
    {
        public static string? GetLatestFieldValue(DataBaseRequestParams requestParams)
        {
            string connectionString = $"Data Source={requestParams.databasePath};Version=3;";
            string? fieldValue = null;

            using (SQLiteConnection connection = new(connectionString))
            {
                connection.Open();

                string query = $"SELECT {requestParams.fieldName} FROM {requestParams.tableName} ORDER BY {requestParams.orderByField} DESC LIMIT 1;";

                using (SQLiteCommand command = new(query, connection))
                {
                    using SQLiteDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        fieldValue = reader[requestParams.fieldName].ToString();
                    }
                }

                connection.Close();
            }

            return fieldValue;
        }
    }
}
