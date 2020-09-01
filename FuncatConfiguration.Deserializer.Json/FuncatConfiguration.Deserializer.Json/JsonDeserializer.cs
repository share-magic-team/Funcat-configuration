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

        public Task<object> DeserializeAsync(Type configurationType, Stream stream, CancellationToken cancellationToken)
        {
            return Task.FromResult(_serializer.Deserialize(new JsonTextReader(new StreamReader(stream)), configurationType));
        }
    }
}