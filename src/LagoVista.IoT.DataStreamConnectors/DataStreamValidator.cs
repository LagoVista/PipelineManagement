﻿using LagoVista.Core.Validation;
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
            /* Update will make sure we have either the access key or secure id which is what we want here, it's fine since this isn't applying the update */
            var validationResult = Validator.Validate(stream);
            if(!validationResult.Successful)
            {
                return validationResult.ToInvokeResult();
            }

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