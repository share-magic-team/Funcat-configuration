using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using FuncatConfiguration.Abstractions;

namespace FuncatConfiguration
{
    public static class ConfigurationManagerBuilderExtensions
    {
        public static async Task<ConfigurationManager> BuildAsync(this ConfigurationManagerBuilder builder, CancellationToken cancellationToken)
        {
            try
            {
                if (builder.Deserializer == null)
                    throw new InvalidOperationException("Deserializer not set");
                if (builder.Storage == null)
                    throw new InvalidOperationException("Storage not set");

                await builder.Storage.InitializeAsync(cancellationToken);

                return new ConfigurationManager(builder.Storage, builder.Deserializer, new ReadOnlyDictionary<string, ConfigurationTypeInfo>(builder.ConfigurationTypeInfos));
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message, e);
            }
        }

        public static ConfigurationManagerBuilder WithConfigurationType(this ConfigurationManagerBuilder builder, Type configurationType, string configurationName, bool cacheConfiguration)
        {
            builder.ConfigurationTypeInfos.Add(configurationName, new ConfigurationTypeInfo(configurationName, configurationType, cacheConfiguration));
            return builder;
        }

        public static ConfigurationManagerBuilder WithConfigurationType<T>(this ConfigurationManagerBuilder builder, string configurationName, bool cacheConfiguration)
        {
            builder.ConfigurationTypeInfos.Add(configurationName, new ConfigurationTypeInfo(configurationName, typeof(T), cacheConfiguration));
            return builder;
        }

        public static ConfigurationManagerBuilder WithConfigurationType<T>(this ConfigurationManagerBuilder builder, bool cacheConfiguration)
        {
            builder.ConfigurationTypeInfos.Add(typeof(T).Name, new ConfigurationTypeInfo(typeof(T), cacheConfiguration));
            return builder;
        }

        public static ConfigurationManagerBuilder WithDeserializer(this ConfigurationManagerBuilder builder, IDeserializer deserializer)
        {
            builder.Deserializer = deserializer;
            return builder;
        }

        public static ConfigurationManagerBuilder WithStorage(this ConfigurationManagerBuilder builder, IStorage storage)
        {
            builder.Storage = storage;
            return builder;
        }
    }
}