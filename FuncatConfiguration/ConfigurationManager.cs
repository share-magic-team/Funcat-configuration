using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FuncatConfiguration.Abstractions;

namespace FuncatConfiguration
{
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

        public Type[] GetAvailableConfigurationTypes()
        {
            return _configurationTypes.Select(x => x.Value.Type).ToArray();
        }

        public async Task<T> GetConfigurationAsync<T>(string name, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Confugration name not set", nameof(name));

            if (!_configurationTypes.TryGetValue(name, out var info))
                throw new InvalidOperationException($"Configuration not registered: [{name}]");

            if (info.CacheConfigration && _cache.TryGetValue(name, out var conf1))
                return (T)conf1;

            var conf2 = await LoadConfigurationAsync<T>(name, cancellationToken);

            if (info.CacheConfigration)
                _cache.Add(name, conf2);

            return conf2;
        }

        public Task<T> GetConfigurationAsync<T>(CancellationToken cancellationToken)
        {
            return GetConfigurationAsync<T>(typeof(T).Name, cancellationToken);
        }

        private async Task<T> LoadConfigurationAsync<T>(string name, CancellationToken cancellationToken)
        {
            using (var stream = await _storage.GetConfigStreamAsync(name, cancellationToken))
                return await _deserializer.DeserializeAsync<T>(stream, cancellationToken);
        }
    }
}