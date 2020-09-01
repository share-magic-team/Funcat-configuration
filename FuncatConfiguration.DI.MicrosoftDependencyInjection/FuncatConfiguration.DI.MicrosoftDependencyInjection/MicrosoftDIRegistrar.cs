using System;
using FuncatConfiguration.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace FuncatConfiguration.DI.MicrosoftDependencyInjection
{
    internal class MicrosoftDIRegistrar : IServiceCollectionRegistrar
    {
        private readonly IServiceCollection _serviceCollection;

        internal MicrosoftDIRegistrar(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public void Register(Type configurationType, Func<object> configurationFactory)
        {
            if (configurationType is null)
                throw new ArgumentNullException(nameof(configurationType));

            if (configurationFactory is null)
                throw new ArgumentNullException(nameof(configurationFactory));

            _serviceCollection.AddTransient(configurationType, (_) => configurationFactory());
        }
    }
}