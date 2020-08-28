using System;

namespace FuncatConfiguration
{
    internal class ConfigurationTypeInfo
    {
        public ConfigurationTypeInfo(string name, Type type, bool cacheConfigration)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type ?? throw new ArgumentNullException(nameof(type));
            CacheConfigration = cacheConfigration;
        }

        public ConfigurationTypeInfo(Type type, bool cacheConfigration)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Name = type.Name;
            CacheConfigration = cacheConfigration;

            if (Type.IsAbstract || Type.IsInterface)
                throw new ArgumentException("Type should be not abstract and not interface", nameof(type));
        }

        public bool CacheConfigration { get; }
        public string Name { get; set; }
        public Type Type { get; set; }
    }
}