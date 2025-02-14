using Rhino.Etl.Async.Tests.Errors;

namespace Rhino.Etl.Async.Tests.Fibonacci.Output
{
    public class OutputFibonacciToDatabaseAsync : EtlAsyncProcess
    {
        private readonly int max;
        private readonly Should should;
        public readonly ThrowingAsyncOperation ThrowingOperation = new ThrowingAsyncOperation();
        public readonly FibonacciAsyncOutput OutputOperation = new FibonacciAsyncOutput();
        public readonly FibonacciDelayAsyncOperation DelayOperation = new FibonacciDelayAsyncOperation();

        public OutputFibonacciToDatabaseAsync(int max, Should should)
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
            Register(DelayOperation);
            Register(OutputOperation);

            return Task.CompletedTask;
        }
    }
}
