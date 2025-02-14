using System.Configuration;
using System.Data;
using Rhino.Etl.Async.Infrastructure;
using Rhino.Etl.Core;
using Rhino.Etl.Core.Infrastructure;
using Rhino.Etl.Core.Operations;

namespace Rhino.Etl.Async.Operations
{
    /// <summary>
    /// Generic input command operation
    /// </summary>
    public abstract class InputCommandAsyncOperation : AbstractCommandAsyncOperation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutputCommandOperation"/> class.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        public InputCommandAsyncOperation(string connectionStringName)
            : this(ConfigurationManager.ConnectionStrings[connectionStringName])
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputCommandOperation"/> class.
        /// </summary>
        /// <param name="connectionStringSettings">Connection string settings to use.</param>
        public InputCommandAsyncOperation(ConnectionStringSettings connectionStringSettings)
            : base(connectionStringSettings)
        {
            UseTransaction = true;
        }

        /// <summary>
        /// Executes this operation
        /// </summary>
        /// <param name="rows">The rows.</param>
        /// <returns></returns>
        public override async IAsyncEnumerable<Row> Execute(IAsyncEnumerable<Row> rows)
        {
            using (IDbConnection connection = Use.Connection(ConnectionStringSettings))
            using (IDbTransaction transaction = BeginTransaction(connection))
            {
                using (currentCommand = connection.CreateCommand())
                {
                    currentCommand.Transaction = transaction;
                    PrepareCommand(currentCommand);
                    using (IDataReader reader = await currentCommand.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            yield return await CreateRowFromReader(reader);
                        }
                    }
                }

                if (transaction != null) transaction.Commit();
            }
        }

        /// <summary>
        /// Creates a row from the reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected abstract Task<Row> CreateRowFromReader(IDataReader reader);

        /// <summary>
        /// Prepares the command for execution, set command text, parameters, etc
        /// </summary>
        /// <param name="cmd">The command.</param>
        protected abstract void PrepareCommand(IDbCommand cmd);
    }
}
