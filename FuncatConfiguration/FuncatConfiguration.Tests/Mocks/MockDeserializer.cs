using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FuncatConfiguration.Abstractions;
using Newtonsoft.Json;

namespace FuncatConfiguration.Tests.Mocks
{
    internal class MockDeserializer : IDeserializer
    {
        public Task<object> DeserializeAsync(Type configurationType, Stream stream, CancellationToken cancellationToken)
        {
            return Task.FromResult(JsonSerializer.Create().Deserialize(new JsonTextReader(new StreamReader(stream)), configurationType));
        }
    }
}