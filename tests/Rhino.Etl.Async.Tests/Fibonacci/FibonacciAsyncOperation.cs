using Rhino.Etl.Async.Operations;
using Rhino.Etl.Core;

namespace Rhino.Etl.Async.Tests.Fibonacci
{
    public class FibonacciAsyncOperation(int max) : AbstractAsyncOperation
    {
        public override async IAsyncEnumerable<Row> Execute(IAsyncEnumerable<Row> rows)
        {
            int a = 0;
            int b = 1;
            var row = new Row();
            row["id"] = 1;
            yield return row;

            for (int i = 0; i < max - 1; i++)
            {
                //Added just to use async
                await Task.Yield();

                int c = a + b;
                row = new Row();
                row["id"] = c;
                yield return row;

                a = b;
                b = c;
            }
        }
    }
}