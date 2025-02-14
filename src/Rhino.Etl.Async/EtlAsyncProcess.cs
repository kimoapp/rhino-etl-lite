using Rhino.Etl.Async.Operations;
using Rhino.Etl.Async.Pipelines;
using Rhino.Etl.Core;

namespace Rhino.Etl.Async
{
    /// <summary>
    /// A single etl process
    /// </summary>
    public abstract class EtlAsyncProcess : EtlAsyncProcessBase<EtlAsyncProcess>, IDisposable
    {
        private IPipelineAsyncExecuter pipelineExecuter = new SingleThreadedNonCachedPipelineAsyncExecuter();

        /// <summary>
        /// Gets the pipeline executer.
        /// </summary>
        /// <value>The pipeline executer.</value>
        public IPipelineAsyncExecuter PipelineExecuter
        {
            get { return pipelineExecuter; }
            set
            {
                pipelineExecuter = value;
            }
        }


        ///// <summary>
        ///// Gets a new partial process that we can work with
        ///// </summary>
        //protected static PartialProcessOperation Partial
        //{
        //    get
        //    {
        //        PartialProcessOperation operation = new PartialProcessOperation();
        //        return operation;
        //    }
        //}

        #region IDisposable Members

        ///<summary>
        ///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ///</summary>
        ///<filterpriority>2</filterpriority>
        public void Dispose()
        {
            foreach (IAsyncOperation operation in operations)
            {
                operation.Dispose();
            }
        }

        #endregion

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        protected abstract Task Initialize();

        /// <summary>
        /// Executes this process
        /// </summary>
        public async Task Execute()
        {
            await Initialize();
            MergeLastOperationsToOperations();
            RegisterToOperationsEvents();
            await PipelineExecuter.Execute(Name, operations, TranslateRows);

            await PostProcessing();
        }

        /// <summary>
        /// Translate the rows from one representation to another
        /// </summary>
        public virtual IAsyncEnumerable<Row> TranslateRows(IAsyncEnumerable<Row> rows)
        {
            return rows;
        }

        private void RegisterToOperationsEvents()
        {
            foreach (IAsyncOperation operation in operations)
            {
                operation.OnRowProcessed += OnRowProcessed;
                operation.OnFinishedProcessing += OnFinishedProcessing;
            }
        }


        /// <summary>
        /// Called when this process has finished processing.
        /// </summary>
        /// <param name="op">The op.</param>
        protected virtual void OnFinishedProcessing(IAsyncOperation op)
        {
        }

        /// <summary>
        /// Allow derived class to deal with custom logic after all the internal steps have been executed
        /// </summary>
        protected virtual Task PostProcessing()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Called when a row is processed.
        /// </summary>
        /// <param name="op">The operation.</param>
        /// <param name="dictionary">The dictionary.</param>
        protected virtual void OnRowProcessed(IAsyncOperation op, Row dictionary)
        {
        }

        /// <summary>
        /// Gets all errors that occured during the execution of this process
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Exception> GetAllErrors()
        {
            foreach (Exception error in Errors)
            {
                yield return error;
            }
            foreach (Exception error in pipelineExecuter.GetAllErrors())
            {
                yield return error;
            }
            foreach (IAsyncOperation operation in operations)
            {
                foreach (Exception exception in operation.GetAllErrors())
                {
                    yield return exception;
                }
            }
        }
    }
}
