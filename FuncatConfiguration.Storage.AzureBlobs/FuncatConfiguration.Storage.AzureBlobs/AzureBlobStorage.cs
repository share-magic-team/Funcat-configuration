using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FuncatConfiguration.Abstractions;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace FuncatConfiguration.Storage.AzureBlobs
{
    internal class AzureBlobStorage : IStorage
    {
        private readonly string _connectionString;
        private readonly string _containerName;
        private readonly string _relativePathInContainer;
        private CloudBlobDirectory _cloudBlobDirectory;
        private CloudBlobContainer _cloudBlobContainer;

        internal AzureBlobStorage(string connectionString, string containerName, string relativePathInContainer)
        {
            _connectionString = connectionString;
            _containerName = containerName;
            _relativePathInContainer = relativePathInContainer;
        }

        public Task<Stream> GetConfigStreamAsync(string configName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(configName))
                throw new ArgumentException("Config name cannot be null or empty string", nameof(configName));

            configName = configName.ToLowerInvariant();

            return FindByName(configName);

            async Task<Stream> FindByName(string name)
            {
                CloudBlob[] items;

                if (_cloudBlobDirectory != null)
                {
                    items = _cloudBlobDirectory
                        .ListBlobs(useFlatBlobListing: true)
                        .OfType<CloudBlob>()
                        .Where(x => x.Name.ToLowerInvariant().StartsWith(configName))
                        .ToArray();
                }
                else
                {
                    items = _cloudBlobContainer
                        .ListBlobs(useFlatBlobListing: true)
                        .OfType<CloudBlob>()
                        .Where(x => x.Name.ToLowerInvariant().StartsWith(configName))
                        .ToArray();
                }

                if (items.Count() == 0)
                    throw new InvalidOperationException($"Cannot find file by pattern: [{name}.*]");
                if (items.Count() > 1)
                    throw new InvalidOperationException($"Multiple files found by pattern: [{name}.*]");

                var stream = new MemoryStream();
                await items[0].DownloadToStreamAsync(stream, cancellationToken);
                return stream;
            }
        }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            var client = CloudStorageAccount.Parse(_connectionString).CreateCloudBlobClient();
            var container = client.GetContainerReference(_containerName);

            if (!await container.ExistsAsync(cancellationToken))
                throw new InvalidOperationException($"Cloud container not exist: [{_containerName}]");

            if (string.IsNullOrEmpty(_relativePathInContainer))
                _cloudBlobContainer = container;
            else
                _cloudBlobDirectory = container.GetDirectoryReference(_relativePathInContainer);
        }
    }
}