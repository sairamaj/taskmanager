using System.Collections.ObjectModel;
using Utils.Core.Model;

namespace Utils.Core.Diagnostics
{
    public interface ILogger
    {
        void Error(string message);
        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Log(LogLevel level, string message);
        void Clear();
        ObservableCollection<LogMessage> LogMessages { get; }
    }
}
