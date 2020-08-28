using System.Collections.Generic;
using FuncatConfiguration.Abstractions;

namespace FuncatConfiguration
{
    public class ConfigurationManagerBuilder
    {
        internal Dictionary<string, ConfigurationTypeInfo> ConfigurationTypeInfos { get; private set; } = new Dictionary<string, ConfigurationTypeInfo>();
        internal IDeserializer Deserializer { get; set; }
        internal IStorage Storage { get; set; }
    }
}