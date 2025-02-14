using Rhino.Etl.Async.Tests.Fibonacci.Output;
using Xunit;

namespace Rhino.Etl.Async.Tests
{
    public class OutputCommandFixture : BaseFibonacciAsyncTest
    {
        [Fact]
        public async Task CanInsertToDatabaseFromInMemoryCollection()
        {
            var fibonaci = new OutputFibonacciToDatabaseAsync(25, Should.WorkFine);
            await fibonaci.Execute();

            Assert25ThFibonacci();
        }

        [Fact]
        public async Task CanInsertToDatabaseFromConnectionStringSettingsAndInMemoryCollection()
        {
            var fibonaci = new OutputFibonacciToDatabaseFromConnectionStringSettingsAsync(25, Should.WorkFine);
            await fibonaci.Execute();

            Assert25ThFibonacci();
        }

        [Fact]
        public async Task WillRaiseRowProcessedEvent()
        {
            int rowsProcessed = 0;

            using (OutputFibonacciToDatabaseAsync fibonaci = new OutputFibonacciToDatabaseAsync(1, Should.WorkFine))
            {
                fibonaci.OutputOperation.OnRowProcessed += delegate { rowsProcessed++; };
                await fibonaci.Execute();
            }

            Assert.Equal(1, rowsProcessed);
        }

        [Fact]
        public async Task WillRaiseRowProcessedEventUntilItThrows()
        {
            int rowsProcessed = 0;

            using (var fibonaci = new OutputFibonacciToDatabaseAsync(25, Should.Throw))
            {
                fibonaci.OutputOperation.OnRowProcessed += delegate { rowsProcessed++; };
                await fibonaci.Execute();

                Assert.Equal(fibonaci.ThrowingOperation.RowsAfterWhichToThrow, rowsProcessed);
            }
        }

        [Fact]
        public async Task WillRaiseFinishedProcessingEventOnce()
        {
            int finished = 0;

            using (var fibonaci = new OutputFibonacciToDatabaseAsync(1, Should.WorkFine))
            {
                fibonaci.OutputOperation.OnFinishedProcessing += delegate { finished++; };
                await fibonaci.Execute();
            }

            Assert.Equal(1, finished);
        }

        [Fact]
        public async Task WhenErrorIsThrownWillRollbackTransaction()
        {
            var fibonaci = new OutputFibonacciToDatabaseAsync(25, Should.Throw);
            await fibonaci.Execute();
            Assert.Equal(1, new List<Exception>(fibonaci.GetAllErrors()).Count);
            AssertFibonacciTableEmpty();
        }
    }
}
