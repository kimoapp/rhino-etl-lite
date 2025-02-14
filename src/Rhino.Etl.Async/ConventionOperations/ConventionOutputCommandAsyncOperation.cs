using System.Configuration;
using System.Data;
using Rhino.Etl.Async.Operations;
using Rhino.Etl.Core;
using Rhino.Etl.Core.Operations;

namespace Rhino.Etl.Async.ConventionOperations
{
    /// <summary>
    /// A convention based version of <see cref="OutputCommandOperation"/>. Will
    /// figure out as many things as it can on its own.
    /// </summary>
    public class ConventionOutputCommandAsyncOperation : OutputCommandAsyncOperation
    {
        private string command;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConventionOutputCommandAsyncOperation"/> class.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        public ConventionOutputCommandAsyncOperation(string connectionStringName)
            : this(ConfigurationManager.ConnectionStrings[connectionStringName])
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConventionOutputCommandAsyncOperation"/> class.
        /// </summary>
        /// <param name="connectionStringSettings">Connection string settings to use.</param>
        public ConventionOutputCommandAsyncOperation(ConnectionStringSettings connectionStringSettings)
            : base(connectionStringSettings)
        {
        }

        /// <summary>
        /// Gets or sets the command to execute against the database
        /// </summary>
        public string Command
        {
            get { return command; }
            set { command = value; }
        }

        /// <summary>
        /// Prepares the row by executing custom logic before passing on to the <see cref="PrepareCommand"/>
        /// for further process.
        /// </summary>
        /// <param name="row">The row.</param>
        protected virtual void PrepareRow(Row row)
        {
        }

        /// <summary>
        /// Prepares the command for execution, set command text, parameters, etc
        /// </summary>
        /// <param name="cmd">The command.</param>
        /// <param name="row">The row.</param>
        protected override void PrepareCommand(IDbCommand cmd, Row row)
        {
            PrepareRow(row);
            cmd.CommandText = Command;
            CopyRowValuesToCommandParameters(currentCommand, row);
        }

    }
}