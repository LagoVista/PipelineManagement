using System;
using System.Threading.Tasks;

namespace LagoVista.IoT.PipelineAdmin.tests.Utils
{
    public class LogWriter : LagoVista.IoT.Logging.Loggers.ILogWriter
    {
        public Task WriteError(Logging.Models.LogRecord record)
        {
            Console.WriteLine(record.Message);
            return Task.FromResult(default(object));
        }

        public Task WriteEvent(Logging.Models.LogRecord record)
        {
            Console.WriteLine(record.Message);
            return Task.FromResult(default(object));
        }
    }
}
