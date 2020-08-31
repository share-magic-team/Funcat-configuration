using System;

namespace FuncatConfiguration
{
    internal class ConfigurationTypeInfo
    {
        public ConfigurationTypeInfo(string name, Type type, bool cacheConfiguration)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type ?? throw new ArgumentNullException(nameof(type));
            CacheConfiguration = cacheConfiguration;
        }

        public ConfigurationTypeInfo(Type type, bool cacheConfiguration)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Name = type.Name;
            CacheConfiguration = cacheConfiguration;

            if (Type.IsAbstract || Type.IsInterface)
                throw new ArgumentException("Type should be not abstract and not interface", nameof(type));
        }

        public bool CacheConfiguration { get; }
        public string Name { get; set; }
        public Type Type { get; set; }
    }
}