using System;
using System.Data.Common;

namespace Badger.Data
{
    sealed class DbQueryBuilder<T> : DbBaseBuilder<IDbQueryBuilder<T>>, IDbQueryBuilder<T>
    {
        private Func<IDbRow, T> mapper;

        public DbQueryBuilder(DbCommand command)
            : base(command)
        {
        }

        public IDbQueryBuilder<T> WithMapper(Func<IDbRow, T> mapper)
        {
            this.mapper = mapper;
            return this;
        }

        public IDbQuery<T> Build()
        {
            return new DbQueryExecuter<T>(command, mapper);
        }
    }
}
