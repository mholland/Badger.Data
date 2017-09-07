using System;
using System.Collections.Generic;

namespace Badger.Data
{
    public interface IDbSession : IDisposable
    {
        int ExecuteCommand(ICommand command);
        IEnumerable<TResult> ExecuteQuery<TResult>(IQuery<TResult> query);
        TResult ExecuteQuery<TResult>(IScalarQuery<TResult> query);
    }
}
