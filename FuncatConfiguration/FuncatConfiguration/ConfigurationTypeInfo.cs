using System;

namespace FuncatConfiguration
{
    public class ConfigurationTypeInfo
    {
        public ConfigurationTypeInfo(string name, Type type, bool cacheConfiguration, bool registerInServiceCollection)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type ?? throw new ArgumentNullException(nameof(type));

            if (Type.IsAbstract || Type.IsInterface)
                throw new ArgumentException("Type should be not abstract and not interface", nameof(type));

            CacheConfiguration = cacheConfiguration;
            RegisterInServiceCollection = registerInServiceCollection;
        }

        public bool CacheConfiguration { get; }
        public string Name { get; }
        public bool RegisterInServiceCollection { get; }
        public Type Type { get; }
    }
}