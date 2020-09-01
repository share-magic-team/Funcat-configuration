using Microsoft.Extensions.DependencyInjection;

namespace FuncatConfiguration.DI.MicrosoftDependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Use Microsoft dependency injection as DI container for configurations
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="serviceCollection">Service collection for services registration</param>
        /// <returns>Builder</returns>
        public static IServiceCollection RegisterConfigurationTypes(this IServiceCollection serviceCollection, ConfigurationManager configurationManager)
        {
            // Some actions
            return serviceCollection;
        }
    }
}