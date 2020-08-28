using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FuncatConfiguration.Abstractions;
using Newtonsoft.Json;

namespace FuncatConfiguration.Tests.Mocks
{
    internal class MockDeserializer : IDeserializer
    {
        public Task<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken)
        {
            return Task.FromResult(JsonSerializer.Create().Deserialize<T>(new JsonTextReader(new StreamReader(stream))));
        }
    }
}