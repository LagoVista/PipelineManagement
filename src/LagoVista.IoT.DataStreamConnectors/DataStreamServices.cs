using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin;
using LagoVista.IoT.Pipeline.Admin.Interfaces;
using LagoVista.IoT.Pipeline.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.DataStreamConnectors
{
    public class DataStreamServices : IDataStreamServices
    {
        IAdminLogger _adminLogger;
        ISecureStorage _secureStorage;
        private readonly Dictionary<string, IDataStreamConnector> _dataStreamConnectors = new Dictionary<string, IDataStreamConnector>();

        public DataStreamServices(IAdminLogger adminLogger, ISecureStorage secureStorage)
        {
            _adminLogger = adminLogger;
            _secureStorage = secureStorage;
        }

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

        public async Task<IDataStreamConnector> GetDataStreamConnectorAsync(DataStream dataStream, EntityHeader instanceUser)
        {
            if (_dataStreamConnectors.ContainsKey(dataStream.Id))
            {
                return _dataStreamConnectors[dataStream.Id];
            }

            var org = dataStream.OwnerOrganization;

            var connectorResult = LagoVista.IoT.DataStreamConnectors.DataStreamServices.GetConnector(dataStream.StreamType.Value, _adminLogger);
            if (connectorResult.Successful)
            {
                switch (dataStream.StreamType.Value)
                {
                    case DataStreamTypes.AzureBlob:
                    case DataStreamTypes.AzureEventHub:
                    case DataStreamTypes.AzureTableStorage:
                    case DataStreamTypes.AzureTableStorage_Managed:
                        var azureSecretKeyResult = await _secureStorage.GetSecretAsync(org, dataStream.AzureAccessKeySecureId, instanceUser);
                        if (azureSecretKeyResult.Successful)
                        {
                            dataStream.AzureAccessKey = azureSecretKeyResult.Result;
                        }

                        break;
                    case DataStreamTypes.Redis:
                        var getSecretResult = await _secureStorage.GetSecretAsync(org, dataStream.RedisPasswordSecureId, instanceUser);
                        if (getSecretResult.Successful)
                        {
                            dataStream.RedisPassword = getSecretResult.Result;
                        }

                        break;

                    case DataStreamTypes.AWSS3:
                    case DataStreamTypes.AWSElasticSearch:
                        var awsSecretKeyResult = await _secureStorage.GetSecretAsync(org, dataStream.AWSSecretKeySecureId, instanceUser);
                        if (awsSecretKeyResult.Successful)
                        {
                            dataStream.AwsSecretKey = awsSecretKeyResult.Result;
                        }

                        break;

                    case DataStreamTypes.PointArrayStorage:
                    case DataStreamTypes.Postgresql:
                    case DataStreamTypes.SQLServer:
                        var dbSecretKeyResult = await _secureStorage.GetSecretAsync(org, dataStream.DBPasswordSecureId, instanceUser);
                        if (dbSecretKeyResult.Successful)
                        {
                            dataStream.DbPassword = dbSecretKeyResult.Result;
                        }

                        break;
                }

                var connector = connectorResult.Result;
                var initResult = await connector.InitAsync(dataStream);
                if (!initResult.Successful)
                {
                    throw new Exception(initResult.Errors.First().Message);
                }
                lock (_dataStreamConnectors)
                {
                    if (!_dataStreamConnectors.ContainsKey(dataStream.Id))
                    {
                        _dataStreamConnectors.Add(dataStream.Id, connector);
                    }
                }
                return connector;
            }
            else
            {
                throw new Exception(connectorResult.Errors.First().Message);
            }
        }
    }
}
