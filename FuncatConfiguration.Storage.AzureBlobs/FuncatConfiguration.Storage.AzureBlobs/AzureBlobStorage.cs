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
        private CloudBlobContainer _cloudBlobContainer;
        private CloudBlobDirectory _cloudBlobDirectory;

        internal AzureBlobStorage(string connectionString, string containerName, string relativePathInContainer)
        {
            _connectionString = connectionString;
            _containerName = containerName;
            _relativePathInContainer = relativePathInContainer;
        }

        public Stream GetConfigStream(string configName)
        {
            if (string.IsNullOrWhiteSpace(configName))
                throw new ArgumentException("Config name cannot be null or empty string", nameof(configName));

            var stream = new MemoryStream();
            var blob = FindBlob(configName);
            blob.DownloadToStream(stream);
            stream.Position = 0;
            return stream;
        }

        public async Task<Stream> GetConfigStreamAsync(string configName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(configName))
                throw new ArgumentException("Config name cannot be null or empty string", nameof(configName));

            var stream = new MemoryStream();
            var blob = FindBlob(configName);
            await blob.DownloadToStreamAsync(stream, cancellationToken);
            stream.Position = 0;
            return stream;
        }

        public void Initialize()
        {
            var client = CloudStorageAccount.Parse(_connectionString).CreateCloudBlobClient();
            var container = client.GetContainerReference(_containerName);

            if (!container.Exists())
                throw new InvalidOperationException($"Cloud container not exist: [{_containerName}]");

            if (string.IsNullOrEmpty(_relativePathInContainer))
                _cloudBlobContainer = container;
            else
                _cloudBlobDirectory = container.GetDirectoryReference(_relativePathInContainer);
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

        private CloudBlob FindBlob(string configName)
        {
            CloudBlob[] items;

            if (_cloudBlobDirectory != null)
            {
                configName = _cloudBlobDirectory.Prefix + configName;

                items = _cloudBlobDirectory
                    .ListBlobs(useFlatBlobListing: true)
                    .OfType<CloudBlob>()
                    .Where(x => x.Name.StartsWith(configName, StringComparison.InvariantCultureIgnoreCase))
                    .ToArray();
            }
            else
            {
                items = _cloudBlobContainer
                    .ListBlobs(useFlatBlobListing: true)
                    .OfType<CloudBlob>()
                    .Where(x => x.Name.StartsWith(configName, StringComparison.InvariantCultureIgnoreCase))
                    .ToArray();
            }

            if (items.Count() == 0)
                throw new InvalidOperationException($"Cannot find file by pattern: [{configName}.*]");
            if (items.Count() > 1)
                throw new InvalidOperationException($"Multiple files found by pattern: [{configName}.*]");

            return items[0];
        }
    }
}