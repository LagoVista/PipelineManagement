using LagoVista.Core.Validation;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin;
using LagoVista.IoT.Pipeline.Admin.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.DataStreamConnectors
{
    public static class DataStreamServices
    {
        public static InvokeResult<IDataStreamConnector> GetConnector (DataStreamTypes type, IAdminLogger logger)
        {
            IDataStreamConnector _streamConnector = null;

            switch (type)
            {
                case DataStreamTypes.AWSElasticSearch: _streamConnector = new AWSElasticSearchConnector(logger); break;
                case DataStreamTypes.AWSS3: _streamConnector = new AWSS3Connector(logger); break;
                case DataStreamTypes.AzureBlob: _streamConnector = new AzureBlobConnector(logger); break;
                case DataStreamTypes.AzureEventHub: _streamConnector = new AzureEventHubConnector(logger); break;
                case DataStreamTypes.AzureTableStorage:
                case DataStreamTypes.AzureTableStorage_Managed: _streamConnector = new AzureTableStorageConnector(logger); break;
                case DataStreamTypes.SQLServer: _streamConnector = new SQLServerConnector(logger); break;
                case DataStreamTypes.Redis: _streamConnector = new RedisConnector(logger); break;
                case DataStreamTypes.Postgresql: _streamConnector = new PostgresqlConnector(logger); break;
                case DataStreamTypes.PointArrayStorage: _streamConnector = new PointArrayPostgresqlConnector(logger); break;
            }

            if (_streamConnector == null) return InvokeResult<IDataStreamConnector>.FromError("Unsupported Stream Type");

            Console.WriteLine($"Created Data stream Connecter {_streamConnector.GetType().Name} for {type} ");

            return  InvokeResult<IDataStreamConnector>.Create(_streamConnector);
        }
    }
}
