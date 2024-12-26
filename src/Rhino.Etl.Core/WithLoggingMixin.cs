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
        /// Logs a warn message
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        protected void Warn(string format, params object[] args)
        {
            //if (log.IsWarnEnabled)
            //{
            //    log.Warn(string.Format(CultureInfo.InvariantCulture, format, args), null);
            //}
        }

        /// <summary>
        /// Logs a debug message
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        protected void Debug(string format, params object[] args)
        {
            //if (log.IsDebugEnabled)
            //{
            //    log.Debug(string.Format(CultureInfo.InvariantCulture, format, args), null);
            //}
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