using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Регистрация всех утилит проекта
        /// </summary>
        /// <param name="serviceCollection">Коллекция служб</param>
        public static void AddIUtility(this IServiceCollection serviceCollection)
        {
            // Получение базового интерфейса 
            var baseInterfaceType = typeof(IUtility<,>);

            // Получение всех интерфейсов и классов
            var interfaceAssemblies = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToList();

            // Получение всех интерфейсов унаследованных от базового за исключением самого базового
            var utilityInterfaces = 
                interfaceAssemblies.Where(x => 
                    x.IsInterface && 
                    x.GetInterfaces()
                        .Any(y => 
                            y.IsGenericType && 
                            y != baseInterfaceType && 
                            y.GetGenericTypeDefinition() == baseInterfaceType
                            )
                        )
                .ToList();

            if (utilityInterfaces == null || utilityInterfaces.Count == 0) return;

            foreach (var iUtility in utilityInterfaces)
            {
                // Получение класса утилиты для текущего интерфейса
                var utilityClass = 
                    interfaceAssemblies
                    .Where(x => !x.IsInterface && iUtility.IsAssignableFrom(x))
                    .ToList();

                if (utilityClass != null && utilityClass.Count > 0) serviceCollection.AddTransient(iUtility, utilityClass.First());
            }
        }
    }
}
