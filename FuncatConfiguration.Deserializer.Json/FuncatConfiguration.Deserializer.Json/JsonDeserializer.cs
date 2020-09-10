using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FuncatConfiguration.Abstractions;
using Newtonsoft.Json;

namespace FuncatConfiguration.Deserializer.Json
{
    internal class JsonDeserializer : IDeserializer
    {
        private readonly JsonSerializer _serializer;

        internal JsonDeserializer(JsonSerializerSettings settings = null)
        {
            _serializer = settings == null ?
                JsonSerializer.Create() :
                JsonSerializer.Create(settings);
        }

        public object Deserialize(Type configurationType, Stream stream)
        {
            return _serializer.Deserialize(new JsonTextReader(new StreamReader(stream)), configurationType);
        }

        public Task<object> DeserializeAsync(Type configurationType, Stream stream, CancellationToken _)
        {
            return Task.FromResult(Deserialize(configurationType, stream));
        }
    }
}