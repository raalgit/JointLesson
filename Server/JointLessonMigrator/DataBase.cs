using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonMigrator
{
    public static class DataBase
    {
        public static void EnsureDatabase(string connectionString, string name)
        {
            var parameters = new DynamicParameters();
            parameters.Add("JointLessonDB", name);
            
            // Создаем подключение к ms sql server
            using var connection = new SqlConnection(connectionString);

            // Отправляем запрос на получение базы данных проекта
            var records = connection.Query("SELECT * FROM sys.databases WHERE name = @name",
                 parameters);

            // Если базы данных нет, то создаем ее
            if (!records.Any())
            {
                connection.Execute($"CREATE DATABASE {name}");
            }
        }
    }
}
