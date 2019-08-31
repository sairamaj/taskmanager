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

        public void Error(string message)
        {
            this.AddMessage(new LogMessage
            {
                DateTime =  DateTime.Now,
                Level = LogLevel.Error,
                Message = message
            });
        }

        public void Debug(string message)
        {
            this.AddMessage(new LogMessage
            {
                DateTime = DateTime.Now,
                Level = LogLevel.Debug,
                Message = message
            });
        }

        public void Info(string message)
        {
            this.AddMessage(new LogMessage
            {
                DateTime = DateTime.Now,
                Level = LogLevel.Info,
                Message = message
            });
        }

        public void Warn(string message)
        {
            this.AddMessage(new LogMessage
            {
                DateTime = DateTime.Now,
                Level = LogLevel.Warn,
                Message = message
            });
        }

        public void Log(LogLevel level, string msg)
        {
            this.AddMessage(new LogMessage
            {
                DateTime = DateTime.Now,
                Level = level,
                Message = msg
            });
        }
    }
    }
