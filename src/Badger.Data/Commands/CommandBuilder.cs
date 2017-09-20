using System.Collections;
using System.Data.Common;
using System.Linq;

namespace Badger.Data.Commands
{
    internal sealed class CommandBuilder : Builder<ICommandBuilder>, ICommandBuilder
    {
        public CommandBuilder(DbCommand command)
            : base (command)
        {
        }

        public ICommandBuilder<T> WithOutputParam<T>()
        {
            return new CommandBuilder<T>(this.command);
        }

        public IPreparedCommand<int> Build()
        {
            return new PreparedCommand(this.command);
        }
    }

    internal sealed class CommandBuilder<T> : ICommandBuilder<T>
    {
        readonly DbCommand command;

        public CommandBuilder(DbCommand command)
        {
            this.command = command;
        }

        public IPreparedCommand<T> Build()
        {
            return new PreparedScalarCommand<T>(this.command);
        }
    }
}
