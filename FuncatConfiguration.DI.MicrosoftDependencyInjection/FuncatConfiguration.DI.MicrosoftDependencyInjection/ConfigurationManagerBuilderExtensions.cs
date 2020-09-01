using Microsoft.Extensions.DependencyInjection;

namespace FuncatConfiguration.DI.MicrosoftDependencyInjection
{
    public static class ConfigurationManagerBuilderExtensions
    {
        /// <summary>
        /// Use Microsoft dependency injection as DI container for configurations
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="serviceCollection">Service collection for services registration</param>
        /// <returns>Builder</returns>
        public static ConfigurationManagerBuilder WithMicrosoftDI(this ConfigurationManagerBuilder builder, IServiceCollection serviceCollection)
        {
            return builder.WithServiceCollectionRegistrar(new MicrosoftDIRegistrar(serviceCollection));
        }
    }
}