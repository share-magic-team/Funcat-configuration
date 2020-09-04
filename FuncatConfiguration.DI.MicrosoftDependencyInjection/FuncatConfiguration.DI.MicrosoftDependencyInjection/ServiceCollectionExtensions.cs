using System.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace FuncatConfiguration.DI.MicrosoftDependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register Funcat configuration types in Microsoft DI
        /// </summary>
        /// <param name="serviceCollection">Service collection for services registration</param>
        /// <param name="configurationManager">Funcat configuration manager</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddConfigurationTypes(this IServiceCollection serviceCollection, ConfigurationManager configurationManager)
        {
            foreach (var info in configurationManager.GetConfigurationTypeInfos())
            {
                if (info.RegisterInServiceCollection)
                    serviceCollection.AddTransient(info.Type, (_) => configurationManager.GetConfigurationAsync(info.Name, info.Type, CancellationToken.None).Result);
            }
            return serviceCollection;
        }
    }
}