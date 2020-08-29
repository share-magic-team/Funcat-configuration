﻿using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FuncatConfiguration.Abstractions;

namespace FuncatConfiguration.Storage.FileSystem
{
    internal class FileSystemStorage : IStorage
    {
        private readonly string _absolutePath;

        private readonly ConcurrentDictionary<string, string> _configFilesPathCache = new ConcurrentDictionary<string, string>();

        internal FileSystemStorage(string folder, bool isRelative = true)
        {
            if (isRelative)
                _absolutePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), folder ?? string.Empty);
            else
                _absolutePath = folder;
        }

        public Task<Stream> GetConfigStreamAsync(string configName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(configName))
                throw new ArgumentException("Configuration name cannot be null, empty or white space", nameof(configName));

            if (_configFilesPathCache.TryGetValue(configName, out var path))
                return Task.FromResult(new FileStream(path, FileMode.Open) as Stream);

            var partialPath = Path.Combine(_absolutePath, configName) + ".";

            var targetConfigFiles = Directory
                .GetFiles(_absolutePath)
                .Where(x => x.StartsWith(partialPath, StringComparison.InvariantCultureIgnoreCase))
                .ToArray();

            if (targetConfigFiles.Count() == 0)
                throw new InvalidOperationException($"Configuration file not found: [{partialPath}*]");

            if (targetConfigFiles.Count() > 1)
                throw new InvalidOperationException($"Too many config files with template: [{partialPath}*]");

            path = targetConfigFiles[0];

            _configFilesPathCache.AddOrUpdate(configName, path, (k, v) => path);

            return Task.FromResult(new FileStream(path, FileMode.Open) as Stream);
        }

        public Task InitializeAsync(CancellationToken cancellationToken)
        {
            if (!Directory.Exists(_absolutePath))
                throw new InvalidOperationException($"Cannot find directory [{_absolutePath}]");

            return Task.CompletedTask;
        }
    }
}