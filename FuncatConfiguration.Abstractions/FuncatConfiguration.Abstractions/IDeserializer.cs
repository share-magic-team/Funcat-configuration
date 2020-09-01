using System;
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
        /// Deserialize configuration from stream into instance of Type
        /// </summary>
        /// <typeparam name="T">Configuration type</typeparam>
        /// <param name="stream">Source of configuration data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task with configuration data instance</returns>
        Task<object> DeserializeAsync(Type configurationType, Stream stream, CancellationToken cancellationToken);
    }
}