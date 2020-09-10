using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FuncatConfiguration.Abstractions;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.File;

namespace FuncatConfiguration.Storage.AzureBlobs
{
    internal class AzureFileShareStorage : IStorage
    {
        private readonly string _connectionString;
        private readonly string _relativePathInShare;
        private readonly string _shareName;
        private CloudFileDirectory _cloudFileDirectory;

        internal AzureFileShareStorage(string connectionString, string shareName, string relativePathInShare = null)
        {
            _connectionString = connectionString;
            _shareName = shareName;
            _relativePathInShare = relativePathInShare;
        }

        public Stream GetConfigStream(string configName)
        {
            if (string.IsNullOrWhiteSpace(configName))
                throw new ArgumentException("Config name cannot be null or empty string", nameof(configName));

            var file = FindFile(configName);
            var stream = new MemoryStream();
            file.DownloadToStream(stream);
            stream.Position = 0;
            return stream;
        }

        public async Task<Stream> GetConfigStreamAsync(string configName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(configName))
                throw new ArgumentException("Config name cannot be null or empty string", nameof(configName));

            var file = FindFile(configName);
            var stream = new MemoryStream();
            await file.DownloadToStreamAsync(stream, cancellationToken);
            stream.Position = 0;
            return stream;
        }

        public void Initialize()
        {
            var client = CloudStorageAccount.Parse(_connectionString).CreateCloudFileClient();
            var cloudFileShare = client.GetShareReference(_shareName);

            if (!cloudFileShare.Exists())
                throw new InvalidOperationException($"Cloud file share not exist: [{_shareName}]");

            _cloudFileDirectory = cloudFileShare.GetRootDirectoryReference();

            if (!string.IsNullOrEmpty(_relativePathInShare))
            {
                var pathElements = _relativePathInShare.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var element in pathElements)
                    _cloudFileDirectory = _cloudFileDirectory.GetDirectoryReference(element);
            }

            if (!_cloudFileDirectory.Exists())
                throw new InvalidOperationException($"Directory not exist: [{_relativePathInShare}]");
        }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            var client = CloudStorageAccount.Parse(_connectionString).CreateCloudFileClient();
            var cloudFileShare = client.GetShareReference(_shareName);

            if (!await cloudFileShare.ExistsAsync(cancellationToken))
                throw new InvalidOperationException($"Cloud file share not exist: [{_shareName}]");

            _cloudFileDirectory = cloudFileShare.GetRootDirectoryReference();

            if (!string.IsNullOrEmpty(_relativePathInShare))
            {
                var pathElements = _relativePathInShare.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var element in pathElements)
                    _cloudFileDirectory = _cloudFileDirectory.GetDirectoryReference(element);
            }

            if (!await _cloudFileDirectory.ExistsAsync(cancellationToken))
                throw new InvalidOperationException($"Directory not exist: [{_relativePathInShare}]");
        }

        private CloudFile FindFile(string configName)
        {
            var items = _cloudFileDirectory
                    .ListFilesAndDirectories(prefix: configName + ".")
                    .OfType<CloudFile>()
                    .ToArray();

            if (items.Count() == 0)
                throw new InvalidOperationException($"Cannot find file by pattern: [{configName}.*]");
            if (items.Count() > 1)
                throw new InvalidOperationException($"Multiple files found by pattern: [{configName}.*]");

            return items[0];
        }
    }
}