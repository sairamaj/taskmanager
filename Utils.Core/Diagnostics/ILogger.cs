using System.Collections.ObjectModel;
using Utils.Core.Model;

namespace Utils.Core.Diagnostics
{
    public interface ILogger
    {
        void Error(string msg);
        void Debug(string msg);
        ObservableCollection<LogMessage> LogMessages { get; }
    }
}
