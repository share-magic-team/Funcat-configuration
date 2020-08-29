using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FuncatConfiguration.Abstractions
{
    /// <summary>
    /// Storage for configuration data
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// Get configuration data stream by configuration name
        /// </summary>
        /// <param name="configName">Configuration name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task with configuration data stream</returns>
        Task<Stream> GetConfigStreamAsync(string configName, CancellationToken cancellationToken);

        /// <summary>
        /// Initialize configuration data storage
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task</returns>
        Task InitializeAsync(CancellationToken cancellationToken);
    }
}