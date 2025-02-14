using Rhino.Etl.Async.Operations;
using Rhino.Etl.Core;

namespace Rhino.Etl.Async.Pipelines
{
    /// <summary>
    /// Base class for pipeline executers, handles all the details and leave the actual
    /// pipeline execution to the 
    /// </summary>
    public abstract class AbstractPipelineAsyncExecuter : WithLoggingMixin, IPipelineAsyncExecuter
    {
        #region IPipelineExecuter Members

        /// <summary>
        /// Executes the specified pipeline.
        /// </summary>
        /// <param name="pipelineName">The name.</param>
        /// <param name="pipeline">The pipeline.</param>
        /// <param name="translateRows">Translate the rows into another representation</param>
        public async Task Execute(string pipelineName,
                            ICollection<IAsyncOperation> pipeline,
                            Func<IAsyncEnumerable<Row>, IAsyncEnumerable<Row>> translateRows)
        {
            try
            {
                var enumerablePipeline = PipelineToEnumerable(
                    pipeline, (new List<Row>()).ToAsyncEnumerable(), translateRows);
                try
                {
                    RaiseNotifyExecutionStarting();
                    await ExecutePipeline(enumerablePipeline);
                    RaiseNotifyExecutionCompleting();
                }
                catch (Exception e)
                {
                    string errorMessage = string.Format("Failed to execute pipeline {0}", pipelineName);
                    Error(e, errorMessage);
                }
            }
            catch (Exception e)
            {
                Error(e, "Failed to create pipeline {0}", pipelineName);
            }

            DisposeAllOperations(pipeline);
        }

        /// <summary>
        /// Transform the pipeline to an enumerable
        /// </summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <param name="rows">The rows</param>
        /// <param name="translateEnumerable">Translate the rows from one representation to another</param>
        /// <returns></returns>
        public virtual async IAsyncEnumerable<Row> PipelineToEnumerable(
            ICollection<IAsyncOperation> pipeline,
            IAsyncEnumerable<Row> rows,
            Func<IAsyncEnumerable<Row>, IAsyncEnumerable<Row>> translateEnumerable)
        {
            foreach (var operation in pipeline)
            {
                await operation.PrepareForExecution(this);
                var enumerator = operation.Execute(rows);
                enumerator = translateEnumerable(enumerator);
                rows = DecorateEnumerableForExecution(operation, enumerator);
            }

            //Needed to satisfy IAsyncEnumerable
            await foreach (var row in rows) yield return row;
            //return rows;
        }

        /// <summary>
        /// Gets all errors that occured under this executer
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Exception> GetAllErrors()
        {
            return Errors;
        }

        /// <summary>
        /// Gets a value indicating whether this instance has errors.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has errors; otherwise, <c>false</c>.
        /// </value>
        public bool HasErrors
        {
            get { return Errors.Length != 0; }
        }

        #endregion

        /// <summary>
        /// Iterates the specified enumerable.
        /// Since we use a pipeline, we need to force it to execute at some point. 
        /// We aren't really interested in the result, just in that the pipeline would execute.
        /// </summary>
        protected virtual async Task ExecutePipeline(IAsyncEnumerable<Row> pipeline)
        {
            var enumerator = pipeline.GetAsyncEnumerator();
            try
            {
#pragma warning disable 642
                while (await enumerator.MoveNextAsync()) ;
#pragma warning restore 642
            }
            catch (Exception e)
            {
                Error(e, "Failed to execute operation {0}", enumerator.Current);
            }
        }


        /// <summary>
        /// Destroys the pipeline.
        /// </summary>
        protected void DisposeAllOperations(ICollection<IAsyncOperation> operations)
        {
            foreach (IAsyncOperation operation in operations)
            {
                try
                {
                    operation.Dispose();
                }
                catch (Exception e)
                {
                    Error(e, "Failed to disposed {0}", operation.Name);
                }
            }
        }

        /// <summary>
        ///    Occurs when    the    pipeline has been successfully created,    but    before it is executed
        /// </summary>
        public event Action<IPipelineAsyncExecuter> NotifyExecutionStarting = delegate { };

        /// <summary>
        ///    Raises the ExecutionStarting event
        /// </summary>
        private void RaiseNotifyExecutionStarting()
        {
            NotifyExecutionStarting(this);
        }

        /// <summary>
        ///    Occurs when    the    pipeline has been successfully created,    but    before it is disposed
        /// </summary>
        public event Action<IPipelineAsyncExecuter> NotifyExecutionCompleting = delegate { };

        /// <summary>
        ///    Raises the ExecutionCompleting event
        /// </summary>
        private void RaiseNotifyExecutionCompleting()
        {
            NotifyExecutionCompleting(this);
        }

        /// <summary>
        /// Add a decorator to the enumerable for additional processing
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="enumerator">The enumerator.</param>
        protected abstract IAsyncEnumerable<Row> DecorateEnumerableForExecution(
            IAsyncOperation operation, IAsyncEnumerable<Row> enumerator);
    }
}