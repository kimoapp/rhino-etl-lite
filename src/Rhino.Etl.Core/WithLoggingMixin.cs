namespace Rhino.Etl.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// A base class that expose easily logging events
    /// </summary>
    public class WithLoggingMixin
    {
        readonly List<Exception> errors = new List<Exception>();
        

        /// <summary>
        /// Logs an error message
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        protected void Error(Exception exception, string format, params object[] args)
        {
            string message = string.Format(CultureInfo.InvariantCulture, format, args);
            string errorMessage;
            if(exception!=null)
                errorMessage = string.Format("{0}: {1}", message, exception.Message);
            else
                errorMessage = message.ToString();
            errors.Add(new RhinoEtlException(errorMessage, exception));
           
        }
        

        /// <summary>
        /// Gets all the errors
        /// </summary>
        /// <value>The errors.</value>
        public Exception[] Errors
        {
            get { return errors.ToArray(); }
        }
    }
}