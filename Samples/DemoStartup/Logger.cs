using System.Diagnostics;

namespace DemoStartup
{
    class Logger : ILogger
    {
        public void Error(string msg)
        {
            Trace.WriteLine($"[ERROR] {msg}");
        }

        public void Debug(string msg)
        {
            Trace.WriteLine($"[DEBUG] {msg}");
        }
    }
}
