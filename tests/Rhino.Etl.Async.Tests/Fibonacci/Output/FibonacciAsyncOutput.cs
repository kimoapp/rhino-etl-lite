using System.Configuration;
using Rhino.Etl.Async.ConventionOperations;

namespace Rhino.Etl.Async.Tests.Fibonacci.Output
{
    public class FibonacciAsyncOutput : ConventionOutputCommandAsyncOperation
    {
        public FibonacciAsyncOutput() : base(DbConnectionForTests.GetForTestDb())
        {
            Command = "INSERT INTO Fibonacci (Id) VALUES(@Id)";
        }

        public FibonacciAsyncOutput(ConnectionStringSettings connectionStringSettings)
            : base(connectionStringSettings)
        {
            Command = "INSERT INTO Fibonacci (Id) VALUES(@Id)";
        }
    }
}