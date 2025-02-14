using Rhino.Etl.Core;
using Rhino.Etl.Core.Operations;

namespace Rhino.Etl.Async.Operations
{
    /// <summary>
    /// Represent a single operation that can occure during the ETL process
    /// </summary>
    public abstract class AbstractAsyncOperation : WithLoggingMixin, IAsyncOperation
    {
        private IPipelineAsyncExecuter pipelineExecuter;

        /// <summary>
        /// Gets the pipeline executer.
        /// </summary>
        /// <value>The pipeline executer.</value>
        protected IPipelineAsyncExecuter PipelineExecuter => pipelineExecuter;

        /// <summary>
        /// Gets the name of this instance
        /// </summary>
        /// <value>The name.</value>
        public virtual string Name => GetType().Name;

        /// <summary>
        /// Gets or sets whether we are using a transaction
        /// </summary>
        /// <value>True or false.</value>
        public bool UseTransaction { get; set; } = true;

        /// <summary>
        /// Gets the statistics for this operation
        /// </summary>
        /// <value>The statistics.</value>
        public OperationStatistics Statistics { get; } = new OperationStatistics();

        /// <summary>
        /// Occurs when a row is processed.
        /// </summary>
        public virtual event Action<IAsyncOperation, Row> OnRowProcessed = delegate { };

        /// <summary>
        /// Occurs when all the rows has finished processing.
        /// </summary>
        public virtual event Action<IAsyncOperation> OnFinishedProcessing = delegate { };

        /// <summary>
        /// Initializes this instance
        /// </summary>
        /// <param name="pipelineExecuter">The current pipeline executer.</param>
        public virtual Task PrepareForExecution(IPipelineAsyncExecuter pipelineExecuter)
        {
            this.pipelineExecuter = pipelineExecuter;
            Statistics.MarkStarted();
            return Task.CompletedTask;
        }

        /// Raises the row processed event
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        void IAsyncOperation.RaiseRowProcessed(Row dictionary)
        {
            Statistics.MarkRowProcessed();
            OnRowProcessed(this, dictionary);
        }

        /// <summary>
        /// Raises the finished processing event
        /// </summary>
        void IAsyncOperation.RaiseFinishedProcessing()
        {
            Statistics.MarkFinished();
            OnFinishedProcessing(this);
        }

        /// <summary>
        /// Gets all errors that occured when running this operation
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<Exception> GetAllErrors()
        {
            return Errors;
        }

        /// <summary>
        /// Executes this operation
        /// </summary>
        /// <param name="rows">The rows.</param>
        /// <returns></returns>
        public abstract IAsyncEnumerable<Row> Execute(IAsyncEnumerable<Row> rows);

        ///<summary>
        ///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ///</summary>
        ///<filterpriority>2</filterpriority>
        public virtual void Dispose()
        {
            
        }
    }
}
