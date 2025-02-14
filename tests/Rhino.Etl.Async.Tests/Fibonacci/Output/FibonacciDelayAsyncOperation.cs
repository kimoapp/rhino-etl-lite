using Rhino.Etl.Async.Operations;
using Rhino.Etl.Core;

namespace Rhino.Etl.Async.Tests.Fibonacci.Output
{
    /// <summary>
    /// Used to test async/await
    /// </summary>
    public class FibonacciDelayAsyncOperation : AbstractAsyncOperation
    {
        public override async IAsyncEnumerable<Row> Execute(IAsyncEnumerable<Row> rows)
        {
            await foreach (var row in rows)
            {
                await Task.Delay(1);
                yield return row;
            }
        }
    }
}