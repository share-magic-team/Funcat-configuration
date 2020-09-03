using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FuncatConfiguration.Abstractions;

namespace FuncatConfiguration
{
    /// <summary>
    /// Configuration types manager
    /// </summary>
    public class ConfigurationManager
    {
        private readonly Dictionary<string, object> _cache = new Dictionary<string, object>();
        private readonly ReadOnlyDictionary<string, ConfigurationTypeInfo> _configurationTypes;
        private readonly IDeserializer _deserializer;
        private readonly IStorage _storage;

        internal ConfigurationManager(IStorage storage, IDeserializer deserializer, ReadOnlyDictionary<string, ConfigurationTypeInfo> configurationTypes)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
            if (configurationTypes == null || configurationTypes.Count == 0)
                throw new ArgumentException("Configuration types are not set");
            else
                _configurationTypes = configurationTypes;
        }

        /// <summary>
        /// Get configuration instance
        /// </summary>
        /// <param name="name">Configration name</param>
        /// <param name="configurationType">Configuration type</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task with configuration instance</returns>
        public async Task<object> GetConfigurationAsync(string name, Type configurationType, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Configuration name not set", nameof(name));

            if (!_configurationTypes.TryGetValue(name, out var info))
                throw new InvalidOperationException($"Configuration not registered: [{name}]");

            if (info.CacheConfiguration && _cache.TryGetValue(name, out var conf1))
                return conf1;

            var conf2 = await LoadConfigurationAsync(configurationType, name, cancellationToken);

            if (info.CacheConfiguration)
                _cache.Add(name, conf2);

            return conf2;
        }

        /// <summary>
        /// Get configuration instance
        /// </summary>
        /// <typeparam name="T">Configuration type</typeparam>
        /// <param name="name">Configration name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task with configuration instance</returns>
        public async Task<T> GetConfigurationAsync<T>(string name, CancellationToken cancellationToken)
        {
            return (T)await GetConfigurationAsync(name, typeof(T), cancellationToken);
        }

        /// <summary>
        /// Get configuration instance
        /// </summary>
        /// <typeparam name="T">Configuration type</typeparam>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task with configuration instance</returns>
        public Task<T> GetConfigurationAsync<T>(CancellationToken cancellationToken)
        {
            return GetConfigurationAsync<T>(Utils.TransformConfigurationName(typeof(T).Name), cancellationToken);
        }

        /// <summary>
        /// Returns array of available configuration types
        /// </summary>
        /// <returns>Array of configuration types</returns>
        public ConfigurationTypeInfo[] GetConfigurationTypeInfos()
        {
            return _configurationTypes.Values.ToArray();
        }

        private async Task<object> LoadConfigurationAsync(Type configurationType, string name, CancellationToken cancellationToken)
        {
            using (var stream = await _storage.GetConfigStreamAsync(name, cancellationToken))
                return await _deserializer.DeserializeAsync(configurationType, stream, cancellationToken);
        }
    }
}