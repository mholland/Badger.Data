using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data
{
    public interface IPreparedCommand<T>
    {
        Task<T> ExecuteAsync(CancellationToken cancellationToken);
        T Execute();
    }
}
