using Rhino.Etl.Async.Enumerables;
using Rhino.Etl.Async.Operations;
using Rhino.Etl.Core;
using Rhino.Etl.Core.Enumerables;
using Rhino.Etl.Core.Operations;

namespace Rhino.Etl.Async.Pipelines
{
    /// <summary>
    /// Execute all the actions syncronously without caching
    /// </summary>
    public class SingleThreadedNonCachedPipelineAsyncExecuter : AbstractPipelineAsyncExecuter
    {
        ///// <summary>
        ///// Add a decorator to the enumerable for additional processing
        ///// </summary>
        ///// <param name="operation">The operation.</param>
        ///// <param name="enumerator">The enumerator.</param>
        //protected override IEnumerable<Row> DecorateEnumerableForExecution(IOperation operation, IEnumerable<Row> enumerator)
        //{
        //    foreach (Row row in new EventRaisingEnumerator(operation, enumerator))
        //    {
        //        yield return row;
        //    }
        //}

        /// <summary>
        /// Add a decorator to the enumerable for additional processing
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="enumerator">The enumerator.</param>
        protected override async IAsyncEnumerable<Row> DecorateEnumerableForExecution(
            IAsyncOperation operation, IAsyncEnumerable<Row> enumerator)
        {
            await foreach (Row row in new EventRaisingAsyncEnumerator(operation, enumerator))
            {
                yield return row;
            }
        }
    }
}