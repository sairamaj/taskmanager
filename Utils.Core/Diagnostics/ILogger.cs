using System.Collections.ObjectModel;
using Utils.Core.Model;

namespace Utils.Core.Diagnostics
{
    /// <summary>
    /// Logger interface.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Gets log message collections.
        /// </summary>
        ObservableCollection<LogMessage> LogMessages { get; }

        /// <summary>
        /// Logs message at error mode.
        /// </summary>
        /// <param name="message">
        /// Error message to be logged.
        /// </param>
        void Error(string message);

        /// <summary>
        /// Logs message at debug mode.
        /// </summary>
        /// <param name="message">
        /// Debug message to be logged.
        /// </param>
        void Debug(string message);

        /// <summary>
        /// Logs message at info mode.
        /// </summary>
        /// <param name="message">
        /// Informational message to be logged.
        /// </param>
        void Info(string message);

        /// <summary>
        /// Logs message at warn mode.
        /// </summary>
        /// <param name="message">
        /// Warning message to be logged.
        /// </param>
        void Warn(string message);

        /// <summary>
        /// Logs message at error mode.
        /// </summary>
        /// <param name="level">
        /// Log level.
        /// </param>
        /// <param name="message">
        /// message to be logged.
        /// </param>
        void Log(LogLevel level, string message);

        /// <summary>
        /// Clears the log messages.
        /// </summary>
        void Clear();
    }
}
