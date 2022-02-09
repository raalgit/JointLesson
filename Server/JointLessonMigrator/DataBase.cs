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
        public static void EnsureDatabase(string connectionString)
        {
            var parameters = new DynamicParameters();
            parameters.Add("name", "JointLessonDB");
            
            // Создаем подключение к ms sql server
            using var connection = new SqlConnection(connectionString);

            // Отправляем запрос на получение базы данных проекта
            var records = connection.Query("SELECT * FROM sys.databases WHERE name = @name",
                 parameters);

            // Если базы данных нет, то создаем ее
            if (!records.Any())
            {
                connection.Execute($"CREATE DATABASE JointLessonDB");
            }
        }
    }
}
