using System;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data
{
    public interface ICommandSession : IDisposable
    {
        T Execute<T>(ICommand<T> command);
        Task<T> ExecuteAsync<T>(ICommand<T> command, CancellationToken cancellationToken);
        Task<T> ExecuteAsync<T>(ICommand<T> command);
        void Commit();
    }
}
