// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 38ca5bff68e71b3e6d6237e97f788fd11bf722f2be64ddbaf10020c7afc93091
// IndexVersion: 2
// --- END CODE INDEX META ---
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
