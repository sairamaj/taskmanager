using System;

namespace Utils.Core.Model
{
    /// <summary>
    /// Log message.
    /// </summary>
    public class LogMessage
    {
        /// <summary>
        /// Gets or sets log level.
        /// </summary>
        public LogLevel Level { get; set; }

        /// <summary>
        /// Gets log level in string format.
        /// </summary>
        public string LevelString => this.Level.ToString();

        /// <summary>
        /// Gets or sets date time.
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Gets or sets log message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets time part of the date.
        /// </summary>
        public string Timestamp => this.DateTime.ToString("HH:mm:ss");
    }
}