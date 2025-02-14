using Rhino.Etl.Core;
using Rhino.Etl.Core.Operations;

namespace Rhino.Etl.Async.Operations
{
    /// <summary>
    /// A single operation in an etl process
    /// </summary>
    public interface IAsyncOperation : IDisposable
    {         
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }

        /// <summary>
        /// Sets the transaction.
        /// </summary>
        /// <value>True or false.</value>
        bool UseTransaction { get; set; }

        /// <summary>
        /// Gets the statistics for this operation
        /// </summary>
        /// <value>The statistics.</value>
        OperationStatistics Statistics { get; }

        /// <summary>
        /// Occurs when a row is processed.
        /// </summary>
        event Action<IAsyncOperation, Row> OnRowProcessed;

        /// <summary>
        /// Occurs when all the rows has finished processing.
        /// </summary>
        event Action<IAsyncOperation> OnFinishedProcessing;

        /// <summary>
        /// Initializes the current instance
        /// </summary>
        /// <param name="pipelineExecuter">The current pipeline executer.</param>
        Task PrepareForExecution(IPipelineAsyncExecuter pipelineExecuter);

        /// <summary>
        /// Executes this operation
        /// </summary>
        /// <param name="rows">The rows.</param>
        IAsyncEnumerable<Row> Execute(IAsyncEnumerable<Row> rows);

        /// <summary>
        /// Raises the row processed event
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        void RaiseRowProcessed(Row dictionary);

        /// <summary>
        /// Raises the finished processing event
        /// </summary>
        void RaiseFinishedProcessing();

        /// <summary>
        /// Gets all errors that occured when running this operation
        /// </summary>
        /// <returns></returns>
        IEnumerable<Exception> GetAllErrors();
    }

}
