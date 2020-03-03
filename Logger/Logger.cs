using System;

namespace Logging
{
    public class Logger
    {
        private static readonly Lazy<FileLogger> logger = new Lazy<FileLogger>(() => new FileLogger("FileConverter"));

        private Logger()
        {
        }

        public static ILogger Instance
        {
            get
            {
                return logger.Value;
            }
        }
    }
}
