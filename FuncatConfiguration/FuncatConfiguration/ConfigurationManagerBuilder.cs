using System.Collections.Generic;
using FuncatConfiguration.Abstractions;

namespace FuncatConfiguration
{
    /// <summary>
    /// Configuration types manager builder
    /// </summary>
    public class ConfigurationManagerBuilder
    {
        internal ConfigurationManagerBuilder()
        {
            // Stub
        }

        internal Dictionary<string, ConfigurationTypeInfo> ConfigurationTypeInfos { get; private set; } = new Dictionary<string, ConfigurationTypeInfo>();

        internal IDeserializer Deserializer { get; set; }

        internal IServiceCollectionRegistrar ServiceCollectionRegistrar { get; set; }

        internal IStorage Storage { get; set; }

        public static ConfigurationManagerBuilder Create() => new ConfigurationManagerBuilder();
    }
}