using System;
using System.Diagnostics;

namespace Logging
{
    public class FileLogger : ILogger
    {
        const int InformationID = 0;
        const int WarningID = 1;
        const int ErrorID = 2;

        public FileLogger(string loggerName)
        {
            Source = new TraceSource(loggerName);
        }

        private TraceSource Source { get; set; }
   
        public void Info(string format, params object[] args)
        {
            Source.TraceEvent(TraceEventType.Information, InformationID, format, args);
        }

        public void Warn(string format, params object[] args)
        {
            Source.TraceEvent(TraceEventType.Warning, WarningID, format, args);
        }

        public void Error(Exception exception)
        {
            Source.TraceEvent(TraceEventType.Error, ErrorID, exception.ToString());
        }

        public void Error(string format, params object[] args)
        {
            Source.TraceEvent(TraceEventType.Error, ErrorID, format, args);
        }
    }
}
