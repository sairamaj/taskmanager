using System;
using System.Collections.ObjectModel;
using Utils.Core.Diagnostics;
using Utils.Core.Model;

namespace Utils.Core.ViewModels
{
    /// <summary>
    /// Log message view model.
    /// </summary>
    public class LogViewModel : ILogger
    {
        /// <summary>
        /// Log messages collection.
        /// </summary>
        private readonly SafeObservableCollection<LogMessage> _logMessages = new SafeObservableCollection<LogMessage>();

        /// <summary>
        /// Gets log message collections.
        /// </summary>
        public ObservableCollection<LogMessage> LogMessages => this._logMessages;

        /// <summary>
        /// Adds a log message to the collection.
        /// </summary>
        /// <param name="logMessage"></param>
        public void AddMessage(LogMessage logMessage)
        {
            this._logMessages.Add(logMessage);
        }

        /// <summary>
        /// Clears the log messages.
        /// </summary>
        public void Clear()
        {
            this._logMessages.Clear();
        }

        /// <summary>
        /// Adds error message.
        /// </summary>
        /// <param name="message">
        /// Error message. 
        /// </param>
        public void Error(string message)
        {
            this.AddMessage(new LogMessage
            {
                DateTime = DateTime.Now,
                Level = LogLevel.Error,
                Message = message,
            });
        }

        /// <summary>
        /// Adds debug message.
        /// </summary>
        /// <param name="message">
        /// Debug message.
        /// </param>
        public void Debug(string message)
        {
            this.AddMessage(new LogMessage
            {
                DateTime = DateTime.Now,
                Level = LogLevel.Debug,
                Message = message,
            });
        }

        /// <summary>
        /// Adds informational message.
        /// </summary>
        /// <param name="message">
        /// Informational message.
        /// </param>
        public void Info(string message)
        {
            this.AddMessage(new LogMessage
            {
                DateTime = DateTime.Now,
                Level = LogLevel.Info,
                Message = message,
            });
        }

        /// <summary>
        /// Adds warning message.
        /// </summary>
        /// <param name="message">
        /// Warning message.
        /// </param>
        public void Warn(string message)
        {
            this.AddMessage(new LogMessage
            {
                DateTime = DateTime.Now,
                Level = LogLevel.Warn,
                Message = message,
            });
        }

        /// <summary>
        /// Adds a generic log message.
        /// </summary>
        /// <param name="level">
        /// Log level.
        /// </param>
        /// <param name="msg">
        /// Message to be logged.
        /// </param>
        public void Log(LogLevel level, string msg)
        {
            this.AddMessage(new LogMessage
            {
                DateTime = DateTime.Now,
                Level = level,
                Message = msg,
            });
        }
    }
}
