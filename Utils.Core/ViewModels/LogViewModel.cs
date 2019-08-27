using System;
using System.Collections.ObjectModel;
using Utils.Core.Diagnostics;
using Utils.Core.Model;

namespace Utils.Core.ViewModels
{
    public class LogViewModel : ILogger
    {
        readonly SafeObservableCollection<LogMessage> _logMessages = new SafeObservableCollection<LogMessage>();

        public ObservableCollection<LogMessage> LogMessages => _logMessages;


        public void AddMessage(LogMessage logMessage)
        {
            _logMessages.Add(logMessage);
        }

        public void Clear()
        {
            _logMessages.Clear();
        }

        public void Error(string msg)
        {
            this.AddMessage(new LogMessage()
            {
                DateTime =  DateTime.Now,
                Level = "Error",
                Message = msg
            });
        }

        public void Debug(string msg)
        {
            this.AddMessage(new LogMessage()
            {
                DateTime = DateTime.Now,
                Level = "Debug",
                Message = msg
            });
        }
    }
}
