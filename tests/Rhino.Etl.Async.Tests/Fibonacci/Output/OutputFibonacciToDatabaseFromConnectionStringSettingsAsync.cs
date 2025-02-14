using Rhino.Etl.Async.Tests.Errors;

namespace Rhino.Etl.Async.Tests.Fibonacci.Output
{
    public class OutputFibonacciToDatabaseFromConnectionStringSettingsAsync : EtlAsyncProcess
    {
        private readonly int max;
        private readonly Should should;
        public readonly ThrowingAsyncOperation ThrowingOperation = new ThrowingAsyncOperation();
        public readonly FibonacciAsyncOutput OutputOperation = new FibonacciAsyncOutput(DbConnectionForTests.GetForTestDb());

        public OutputFibonacciToDatabaseFromConnectionStringSettingsAsync(int max, Should should)
        {
            this.max = max;
            this.should = should;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        protected override Task Initialize()
        {
            Register(new FibonacciAsyncOperation(max));
            if (should == Should.Throw)
                Register(ThrowingOperation);
            Register(OutputOperation);

            return Task.CompletedTask;
        }
    }
}
