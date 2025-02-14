using System.Configuration;
using System.Data;
using Rhino.Etl.Core.Operations;

namespace Rhino.Etl.Async.Operations
{
    /// <summary>
    /// Base class for operations that directly manipulate ADO.Net
    /// It is important to remember that this is supposed to be a deep base class, not to be 
    /// directly inherited or used
    /// </summary>
    public abstract class AbstractCommandAsyncOperation : AbstractDatabaseAsyncOperation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="connectionStringName"/> class.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        protected AbstractCommandAsyncOperation(string connectionStringName)
            : this(ConfigurationManager.ConnectionStrings[connectionStringName])
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractDatabaseOperation"/> class.
        /// </summary>
        /// <param name="connectionStringSettings">The connection string settings to use.</param>
        protected AbstractCommandAsyncOperation(ConnectionStringSettings connectionStringSettings)
            : base(connectionStringSettings)
        {
        }

        /// <summary>
        /// The current command
        /// </summary>
        protected IDbCommand currentCommand;

        /// <summary>
        /// Adds the parameter to the current command
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        protected void AddParameter(string name, object value)
        {
            AddParameter(currentCommand, name, value);
        }

    }
}