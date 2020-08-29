using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FuncatConfiguration.Abstractions
{
    /// <summary>
    /// Deserializer for configuration data
    /// </summary>
    public interface IDeserializer
    {
        /// <summary>
        /// Deserialize configuration from stream into instance of T
        /// </summary>
        /// <typeparam name="T">Configuration type</typeparam>
        /// <param name="stream">Source of configuration data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task with configuration data instance</returns>
        Task<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken);
    }
}