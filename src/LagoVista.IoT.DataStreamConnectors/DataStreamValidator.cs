using LagoVista.Core.Validation;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin;
using LagoVista.IoT.Pipeline.Admin.Models;
using System;
using System.Collections.Generic;
using System.Text;
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

            IDataStreamConnector _streamConnector = null;

            switch (stream.StreamType.Value)
            {
                case DataStreamTypes.AWSElasticSearch: _streamConnector = new AWSElasticSearchConnector(logger); break;
                case DataStreamTypes.AWSS3: _streamConnector = new AWSS3Connector(logger); break;
                case DataStreamTypes.AzureBlob: _streamConnector = new AzureBlobConnector(logger); break;
                case DataStreamTypes.AzureEventHub: _streamConnector = new AzureEventHubConnector(logger); break;
                case DataStreamTypes.AzureTableStorage: 
                case DataStreamTypes.AzureTableStorage_Managed: _streamConnector = new AzureTableStorageConnector(logger); break;
                case DataStreamTypes.SQLServer: _streamConnector = new SQLServerConnector(logger); break;
            }

            if(_streamConnector == null) return InvokeResult.FromError("Unsupported Stream Type");

            try
            {
                var result =  await _streamConnector.ValidateConnectionAsync(stream);
                return result;
            }
            catch(Exception ex)
            {
                return InvokeResult.FromException("DataStreamValidator_ValidateDataStreamAsync", ex);
            }
        }
    }
}
