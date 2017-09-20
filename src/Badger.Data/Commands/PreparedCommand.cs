using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data.Commands
{
    internal sealed class PreparedCommand : IPreparedCommand<int>
    {
        private readonly DbCommand command;

        public PreparedCommand(DbCommand command)
        {
            this.command = command;
        }

        public int Execute()
        {
            return this.command.ExecuteNonQuery();
        }

        public async Task<int> ExecuteAsync(CancellationToken cancellationToken)
        {
            return await this.command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    internal sealed class PreparedScalarCommand<T> : IPreparedCommand<T>
    {
        private readonly DbCommand command;

        public PreparedScalarCommand(DbCommand command)
        {
            this.command = command;
        }

        public T Execute()
        {
            return (T)this.command.ExecuteScalar();
        }

        public async Task<T> ExecuteAsync(CancellationToken cancellationToken)
        {
            return (T)await this.command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
