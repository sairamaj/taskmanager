using System;

namespace Utils.Core.Model
{
    public class LogMessage
    {
        public string Level { get; set; }
        public string LevelString { get; set; }
        public DateTime DateTime { get; set; }
        public string Message { get; set; }
        public string Time => DateTime.ToString("HH:mm:ss");
    }
}
                                       