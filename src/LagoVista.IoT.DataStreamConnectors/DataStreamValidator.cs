// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: cf7b8f0ddb44cd992a51130bd5416de53d52f0b48df5ff3d40f26874b4b94234
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin.Models;
using System;
using System.Threading.Tasks;

namespace LagoVista.IoT.DataStreamConnectors
{
    public static class DataStreamValidator
    {
        public static async Task<InvokeResult> ValidateDataStreamAsync(DataStream stream, IAdminLogger logger)
        {
            var result = DataStreamServices.GetConnector(stream.StreamType.Value, logger);
            if (!result.Successful) return result.ToInvokeResult();
            
            try
            {
                return await result.Result.ValidateConnectionAsync(stream);
            }
            catch(Exception ex)
            {
                return InvokeResult.FromException("DataStreamValidator_ValidateDataStreamAsync", ex);
            }
        }
    }
}
