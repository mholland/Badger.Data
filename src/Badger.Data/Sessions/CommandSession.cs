using System.Data;
using System.Data.Common;
using Badger.Data.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data.Sessions
{
    internal sealed class CommandSession : Session, ICommandSession
    {
        public CommandSession(DbConnection connection, IsolationLevel isolationLevel)
            : base(connection, isolationLevel)
        {
        }

        public T Execute<T>(ICommand<T> command)
        {
            var builder = new CommandBuilder(CreateCommand());
            return command.Prepare(builder).Execute();
        }

        public async Task<T> ExecuteAsync<T>(ICommand<T> command, CancellationToken cancellationToken)
        {
            var builder = new CommandBuilder(await CreateCommandAsync(cancellationToken));
            return await command.Prepare(builder).ExecuteAsync(cancellationToken).ConfigureAwait(false);
  
        }

        public Task<T> ExecuteAsync<T>(ICommand<T> command)
        {
            return ExecuteAsync(command, CancellationToken.None);
        }
    }
}
