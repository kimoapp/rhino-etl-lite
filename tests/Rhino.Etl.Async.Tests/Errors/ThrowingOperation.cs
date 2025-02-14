using Rhino.Etl.Async.Operations;
using Rhino.Etl.Core;

namespace Rhino.Etl.Async.Tests.Errors
{
    public class ThrowingAsyncOperation : AbstractAsyncOperation
    {
        private readonly int rowsAfterWhichToThrow = new Random().Next(1, 6);

        public int RowsAfterWhichToThrow
        {
            get { return rowsAfterWhichToThrow; }
        }

        public override async IAsyncEnumerable<Row> Execute(IAsyncEnumerable<Row> rows)
        {
            for (int i = 0; i < RowsAfterWhichToThrow; i++)
            {
                //Added just to use async
                await Task.Yield();

                Row row = new Row();
                row["id"] = i;
                yield return row;
            }
            throw new InvalidDataException("problem");
        }
    }
}
