using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using FuncatConfiguration.Abstractions;

namespace FuncatConfiguration
{
    /// <summary>
    /// Extensions for configuration manager builder
    /// </summary>
    public static class ConfigurationManagerBuilderExtensions
    {
        /// <summary>
        /// Asynchronously build configuration manager
        /// </summary>
        /// <param name="builder">Builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Configuration manager</returns>
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

        /// <summary>
        /// Register configuration type
        /// </summary>
        /// <param name="builder">Builder</param>
        /// <param name="configurationType">Type of configuration</param>
        /// <param name="configurationName">Name of configuration</param>
        /// <param name="cacheConfiguration">If true - cache configuration instance, else - load configuration from storage every time</param>
        /// <returns>Builder</returns>
        public static ConfigurationManagerBuilder WithConfigurationType(this ConfigurationManagerBuilder builder, Type configurationType, string configurationName, bool cacheConfiguration = true, bool registerInServiceCollection = true)
        {
            builder.ConfigurationTypeInfos.Add(configurationName, new ConfigurationTypeInfo(configurationName, configurationType, cacheConfiguration, registerInServiceCollection));
            return builder;
        }

        /// <summary>
        /// Register configuration type
        /// </summary>
        /// <typeparam name="T">Type of configuration</typeparam>
        /// <param name="builder">Builder</param>
        /// <param name="configurationName">Name of configuration</param>
        /// <param name="cacheConfiguration">If true - cache configuration instance, else - load configuration from storage every time</param>
        /// <returns>Builder</returns>
        public static ConfigurationManagerBuilder WithConfigurationType<T>(this ConfigurationManagerBuilder builder, string configurationName, bool cacheConfiguration = true, bool registerInServiceCollection = true)
        {
            builder.ConfigurationTypeInfos.Add(configurationName, new ConfigurationTypeInfo(configurationName, typeof(T), cacheConfiguration, registerInServiceCollection));
            return builder;
        }

        /// <summary>
        /// Register configuration type
        /// </summary>
        /// <typeparam name="T">Type of configuration</typeparam>
        /// <param name="builder">Builder</param>
        /// <param name="cacheConfiguration">If true - cache configuration instance, else - load configuration from storage every time</param>
        /// <returns>Builder</returns>
        public static ConfigurationManagerBuilder WithConfigurationType<T>(this ConfigurationManagerBuilder builder, bool cacheConfiguration = true, bool registerInServiceCollection = true)
        {
            builder.ConfigurationTypeInfos.Add(typeof(T).Name, new ConfigurationTypeInfo(typeof(T), cacheConfiguration, registerInServiceCollection));
            return builder;
        }

        /// <summary>
        /// Register deserializer for configuration data
        /// </summary>
        /// <param name="builder">Builder</param>
        /// <param name="deserializer">Deserializer instance</param>
        /// <returns>Builder</returns>
        public static ConfigurationManagerBuilder WithDeserializer(this ConfigurationManagerBuilder builder, IDeserializer deserializer)
        {
            if (deserializer is null)
                throw new ArgumentNullException(nameof(deserializer));

            if (builder.Deserializer != null)
                throw new InvalidOperationException("Deserializer already set");

            builder.Deserializer = deserializer;
            return builder;
        }

        /// <summary>
        /// Register DI container registrar
        /// </summary>
        /// <param name="builder">Builder</param>
        /// <param name="serviceCollectionRegistrar">Class that represents functionality to register configuration files in DI container</param>
        /// <returns></returns>
        public static ConfigurationManagerBuilder WithServiceCollectionRegistrar(this ConfigurationManagerBuilder builder, IServiceCollectionRegistrar serviceCollectionRegistrar)
        {
            if (serviceCollectionRegistrar is null)
                throw new ArgumentNullException(nameof(serviceCollectionRegistrar));

            if (builder.ServiceCollectionRegistrar != null)
                throw new InvalidOperationException("ServiceCollectionRegistrar already set");

            builder.ServiceCollectionRegistrar = serviceCollectionRegistrar;
            return builder;
        }

        /// <summary>
        /// Register storage for configuration data
        /// </summary>
        /// <param name="builder">Builder</param>
        /// <param name="storage">Configuration storage instance</param>
        /// <returns>Builder</returns>
        public static ConfigurationManagerBuilder WithStorage(this ConfigurationManagerBuilder builder, IStorage storage)
        {
            if (storage is null)
                throw new ArgumentNullException(nameof(storage));

            if (builder.Storage != null)
                throw new InvalidOperationException("Storage already set");

            builder.Storage = storage;
            return builder;
        }
    }
}