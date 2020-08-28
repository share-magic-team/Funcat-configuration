using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FuncatConfiguration.Abstractions
{
    public interface IStorage
    {
        Task<Stream> GetConfigStreamAsync(string configName, CancellationToken cancellationToken);

        Task InitializeAsync(CancellationToken cancellationToken);
    }
}