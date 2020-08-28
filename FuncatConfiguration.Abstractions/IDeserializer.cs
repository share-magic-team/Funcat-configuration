using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FuncatConfiguration.Abstractions
{
    public interface IDeserializer
    {
        Task<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken);
    }
}