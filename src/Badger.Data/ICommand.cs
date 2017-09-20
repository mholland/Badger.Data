namespace Badger.Data
{
    /// <summary>
    /// A command that can be executed.
    /// </summary>
    public interface ICommand : ICommand<int>
    {
    }

    public interface ICommand<T>
    {
        /// <summary>
        /// Prepars the command so that it can be executed.
        /// </summary>
        /// <param name="commandBuilder">a builder to help create the prepared command.</param>
        IPreparedCommand<T> Prepare(ICommandBuilder commandBuilder);
    }
}
