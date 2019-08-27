using System;

namespace Utils.Core.Model
{
    public class LogMessage
    {
        public LogLevel Level { get; set; }

        public string LevelString => Level.ToString();

        public DateTime DateTime { get; set; }
        public string Message { get; set; }
        public string Timestamp => DateTime.ToString("HH:mm:ss");
    }
}
                                       