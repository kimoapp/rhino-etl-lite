using System.Data;
using Xunit;
using Rhino.Etl.Core.Infrastructure;

namespace Rhino.Etl.Tests
{
    public class BaseFibonacciTest
    {
        public BaseFibonacciTest()
        {
            Use.Transaction(DbConnectionForTests.GetForTestDb(), delegate(IDbCommand cmd)
            {
                cmd.CommandText =
                    @"
if object_id('Fibonacci') is not null
    drop table Fibonacci
create table Fibonacci ( id int );
";
                cmd.ExecuteNonQuery();
            });
        }

        protected static void Assert25ThFibonacci()
        {
            int max = Use.Transaction(DbConnectionForTests.GetForTestDb(), delegate(IDbCommand cmd)
            {
                cmd.CommandText = "SELECT MAX(id) FROM Fibonacci";
                return (int) cmd.ExecuteScalar();
            });
            Assert.Equal(75025, max);
        }

        protected static void AssertFibonacciTableEmpty()
        {
            int count = Use.Transaction(DbConnectionForTests.GetForTestDb(), delegate(IDbCommand cmd)
            {
                cmd.CommandText = "SELECT count(id) FROM Fibonacci";
                return (int) cmd.ExecuteScalar();
            });
            Assert.Equal(0, count);
        }
    }
}