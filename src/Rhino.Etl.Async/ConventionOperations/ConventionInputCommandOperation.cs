using System.Configuration;
using System.Data;
using Rhino.Etl.Async.Operations;
using Rhino.Etl.Core;
using Rhino.Etl.Core.ConventionOperations;
using Rhino.Etl.Core.Operations;

namespace Rhino.Etl.Async.ConventionOperations
{
    /// <summary>
    /// A convention based version of <see cref="InputCommandOperation"/>. Will
    /// figure out as many things as it can on its own.
    /// </summary>
    public class ConventionInputCommandAsyncOperation : InputCommandAsyncOperation
    {
        private string command;
        private int timeout;

        /// <summary>
        /// Gets or sets the command to get the input from the database
        /// </summary>
        public string Command
        {
            get { return command; }
            set { command = value; }
        }

        ///<summary>
        /// Gets or sets the timeout value for the database command
        ///</summary>
        public int Timeout
        {
            get { return timeout;  }
            set { timeout = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConventionInputCommandOperation"/> class.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        public ConventionInputCommandAsyncOperation(string connectionStringName) : this(ConfigurationManager.ConnectionStrings[connectionStringName])
        {
            Timeout = 30;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConventionInputCommandOperation"/> class.
        /// </summary>
        /// <param name="connectionStringSettings">Name of the connection string.</param>
        public ConventionInputCommandAsyncOperation(ConnectionStringSettings connectionStringSettings)
            : base(connectionStringSettings)
        {
        }

        /// <summary>
        /// Creates a row from the reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected override Task<Row> CreateRowFromReader(IDataReader reader)
        {
            return Task.FromResult(Row.FromReader(reader));
        }

        /// <summary>
        /// Prepares the command for execution, set command text, parameters, etc
        /// </summary>
        /// <param name="cmd">The command.</param>
        protected override void PrepareCommand(IDbCommand cmd)
        {
            cmd.CommandText = Command;
            cmd.CommandTimeout = Timeout;
        }
    }
}