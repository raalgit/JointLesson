using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;

namespace JointLessonMigrator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Начало работы мигратора");

            Console.WriteLine("Установка корневой директории");
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            Console.WriteLine("Получение данных о среде проета");
            string environment = Environment.GetEnvironmentVariable("environment");

            Console.WriteLine($"Установлена среда {environment}");

            Console.WriteLine($"Получение конфигурации среды");
            IConfiguration configuration = getConfiguration(environment);

            Console.WriteLine("Получение строки подключения к базе данных");
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            Console.WriteLine("Инициализация сервиса мигратора");
            IServiceCollection services = new ServiceCollection()
                .AddLogging(x => x.AddFluentMigratorConsole())
                .AddFluentMigratorCore()
                .ConfigureRunner(x => x
                    .AddSqlServer2016()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(Assembly.GetExecutingAssembly()).For.All());

            var provider = services.BuildServiceProvider(false);
            using var scope = provider.CreateScope();

            Console.WriteLine("Запуск миграций");
            scope.ServiceProvider.GetRequiredService<IMigrationRunner>().MigrateUp();

            Console.WriteLine("Миграции успешно выполнены");
        }

        static IConfiguration getConfiguration(string environment)
        {
            if (string.IsNullOrEmpty(environment))
                throw new ArgumentNullException(nameof(environment));

            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory);

            switch (environment)
            {
                case "local":
                    configurationBuilder.AddJsonFile("appsettings-local.json", true, true);
                    break;
                case "test":
                    configurationBuilder.AddJsonFile("appsettings-test.json", true, true);
                    break;
            }

            return configurationBuilder.Build();
        }
    }
}
