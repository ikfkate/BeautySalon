using System;
using System.Data;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace DAL
{
    public class DatabaseHelper
    {
        // Строка подключения 
        //private string connectionString = "Host=localhost;Port=5432;Database=MyDB;Username=postgres;Password=admin;";
        private string connectionString;

        public DatabaseHelper()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory) // <-- ИСПОЛЬЗУЕМ ЭТОТ ПУТЬ
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = builder.Build();
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Метод для получения таблицы с данными
        public DataTable ExecuteQuery(string query)
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader); // Автоматически загружаем данные в таблицу
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка исключительной ситуации (требование методички)
                throw new Exception("Ошибка при подключении к базе данных: " + ex.Message);
            }

            return dataTable;
        }
    }
}