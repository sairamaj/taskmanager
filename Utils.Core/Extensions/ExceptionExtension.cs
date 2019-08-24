using System;
using System.Linq;
using System.Reflection;

namespace Utils.Core.Extensions
{
    /// <summary>
    /// The exception extension.
    /// </summary>
    public static class ExceptionExtension
    {
        /// <summary>
        /// The get exception details.
        /// </summary>
        /// <param name="exception">
        /// The e.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="exception"/> is <see language="null"/>.
        /// </exception>
        public static string GetExceptionDetails(this Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            var msg = exception.Message;
            var reflectionException = exception as ReflectionTypeLoadException;
            if (reflectionException != null)
            {
                msg += "\r\n\t";
                msg = reflectionException.LoaderExceptions.Aggregate(msg, (current, ex) => current + ex.GetExceptionDetails());
            }

            msg += "\r\n\t";
            msg += exception.StackTrace;
            if (exception.InnerException != null)
            {
                msg += "\r\n=====================================";
                msg += exception.InnerException.GetExceptionDetails();
            }

            return msg;
        }

        /// <summary>
        /// The get exception message.
        /// </summary>
        /// <param name="exception">
        /// The e.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="exception"/> is <see language="null"/>.
        /// </exception>
        public static string GetExceptionMessage(this Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            var msg = exception.Message;
            if (exception is ReflectionTypeLoadException reflectionException)
            {
                msg += "\r\n\t";
                msg = reflectionException.LoaderExceptions.Aggregate(msg, (current, ex) => current + ex.GetExceptionDetails());
            }

            msg += "\r\n\t";
            if (exception.InnerException == null)
            {
                return msg;
            }

            msg += "=====================================\r\n\t";
            msg += exception.InnerException.GetExceptionMessage();
            return msg;
        }

        /// <summary>
        /// Gets inner most exception.
        /// </summary>
        /// <param name="exception">Exception object.</param>
        /// <returns>Inner exception.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="exception"/> is <see language="null"/>.
        /// </exception>
        public static Exception GetInnermostException(this Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            if (exception.InnerException != null)
            {
                return exception.InnerException.GetInnermostException();
            }
            else
            {
                return exception;
            }
        }
    }
}
