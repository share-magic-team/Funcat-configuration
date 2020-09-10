using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FuncatConfiguration.Abstractions;

namespace FuncatConfiguration.Tests.Mocks
{
    internal class MockStorage : IStorage
    {
        private readonly Dictionary<string, string> _configsAndJson;

        public MockStorage(Dictionary<string, string> configsAndJson)
        {
            _configsAndJson = configsAndJson;
        }

        public Stream GetConfigStream(string configName)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(_configsAndJson[configName]));
        }

        public Task<Stream> GetConfigStreamAsync(string configName, CancellationToken cancellationToken)
        {
            return Task.FromResult((Stream)new MemoryStream(Encoding.UTF8.GetBytes(_configsAndJson[configName])));
        }

        public void Initialize()
        {
            // Do nothings
        }

        public Task InitializeAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}