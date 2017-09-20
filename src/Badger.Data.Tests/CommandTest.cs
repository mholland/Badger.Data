using Dapper;
using Shouldly;
using System;
using Microsoft.Data.Sqlite;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;

namespace Badger.Data.Tests
{
    public abstract class CommandTest<T> : IClassFixture<T> where T : DbTestFixture
    {
        private readonly T fixture;
        private readonly ISessionFactory sessionFactory;

        protected CommandTest(T fixture)
        {
            this.fixture = fixture;

            this.sessionFactory = new SessionFactory(fixture.ProviderFactory, this.fixture.ConnectionString);
        }

        class InsertPersonCommand : ICommand
        {
            private readonly string name;
            private readonly DateTime dob;

            public InsertPersonCommand(string name, DateTime dob)
            {
                this.name = name;
                this.dob = dob;
            }
            
            public IPreparedCommand<int> Prepare(ICommandBuilder builder)
            {
                return builder
                    .WithSql("insert into people(name, dob) values (@name, @dob)")
                    .WithParameter("name", this.name)
                    .WithParameter("dob", this.dob)
                    .Build();
            }
        }

        [Fact]
        public void SessionInsertShouldAlterOneRow()
        {
            var name = Guid.NewGuid().ToString();
            var dob = new DateTime(1990, 5, 20);

            using (var session = this.sessionFactory.CreateCommandSession())
            {
                session.Execute(new InsertPersonCommand(name, dob)).ShouldBe(1);
                session.Commit();
            }
        
            var result = this.fixture.Connection.QuerySingle<Person>(
                "select name, dob from people where name = @name", new { name });

            result.Dob.ShouldBe(dob);
        }

        [Fact]
        public async Task SessionInsertShouldAlterOneRowAsync()
        {
            var name = Guid.NewGuid().ToString();
            var dob = new DateTime(1990, 5, 20);

            using (var session = this.sessionFactory.CreateCommandSession())
            {
                (await session.ExecuteAsync(new InsertPersonCommand(name, dob))).ShouldBe(1);
                session.Commit();
            }
        
            var result = this.fixture.Connection.QuerySingle<Person>(
                "select name, dob from people where name = @name", new { name });

            result.Dob.ShouldBe(dob);
        }

        [Fact]
        public void UncommitedTransactionSessionInsertShouldNotAlterAnyRows()
        {
            var name = Guid.NewGuid().ToString();
            var dob = new DateTime(1990, 5, 20);

            using (var session = this.sessionFactory.CreateCommandSession())
            {
                session.Execute(new InsertPersonCommand(name, dob)).ShouldBe(1);
            }
        
            var result = this.fixture.Connection.QuerySingleOrDefault<Person>(
                "select name, dob from people where name = @name", new { name });

            result.ShouldBeNull();
        }

        [Fact]
        public async Task UncommitedTransactionSessionInsertShouldNotAlterAnyRowsAsync()
        {
            var name = Guid.NewGuid().ToString();
            var dob = new DateTime(1990, 5, 20);

            using (var session = this.sessionFactory.CreateCommandSession())
            {
                (await session.ExecuteAsync(new InsertPersonCommand(name, dob))).ShouldBe(1);
            }
        
            var result = this.fixture.Connection.QuerySingleOrDefault<Person>(
                "select name, dob from people where name = @name", new { name });

            result.ShouldBeNull();
        }

        [Fact]
        public void CommandSessionWithNoExecutionsDoesNotThrow()
        {
            this.sessionFactory.CreateCommandSession().Dispose();
        }

        [Fact]
        public void CommittedCommandSessionWithNoExecutionsDoesNotThrow()
        {
            using (var session = this.sessionFactory.CreateCommandSession())
            {
                session.Commit();
            }
        }
    }
}
