using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LagoVista.Core.Exceptions;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Managers;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Pipeline.Admin.Repos;
using LagoVista.IoT.Pipeline.Admin.Resources;

namespace LagoVista.IoT.Pipeline.Admin.Managers
{
    public class DataStreamManager : ManagerBase, IDataStreamManager
    {
        IDataStreamRepo _dataStreamRepo;
        ISecureStorage _secureStorage;
        IDefaultInternalDataStreamConnectionSettings _defaultConnectionSettings;

        public DataStreamManager(IDataStreamRepo dataStreamRepo, IDefaultInternalDataStreamConnectionSettings defaultConnectionSettings, IAdminLogger logger, ISecureStorage secureStorage, IAppConfig appConfig, IDependencyManager depmanager, ISecurity security) :
            base(logger, appConfig, depmanager, security)
        {
            _dataStreamRepo = dataStreamRepo;
            _secureStorage = secureStorage;
            _defaultConnectionSettings = defaultConnectionSettings;
        }

        public async Task<InvokeResult> AddDataStreamAsync(DataStream stream, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(stream, AuthorizeResult.AuthorizeActions.Create, user, org);
            if (stream.StreamType.Value == DataStreamTypes.AzureTableStorage_Managed)
            {
                stream.AzureStorageAccountName = _defaultConnectionSettings.DefaultInternalDataStreamConnectionSettingsTableStorage.AccountId;
                stream.AzureAccessKey = _defaultConnectionSettings.DefaultInternalDataStreamConnectionSettingsTableStorage.AccessKey;
            }

            ValidationCheck(stream, Actions.Create);

            if (stream.StreamType.Value == DataStreamTypes.AzureBlob ||
                stream.StreamType.Value == DataStreamTypes.AzureEventHub ||
                stream.StreamType.Value == DataStreamTypes.AzureTableStorage ||
                stream.StreamType.Value == DataStreamTypes.AzureTableStorage_Managed)
            {
                if (!String.IsNullOrEmpty(stream.AzureAccessKey))
                {
                    var addSecretResult = await _secureStorage.AddSecretAsync(stream.AzureAccessKey);
                    if (!addSecretResult.Successful) return addSecretResult.ToInvokeResult();
                    stream.AzureAccessKeySecureId = addSecretResult.Result;
                    stream.AzureAccessKey = null;
                }
                else
                {
                    throw new Exception("Validation should have cut null or empty AzureAccessKey, but it did not.");
                }
            }
            else if (stream.StreamType.Value == DataStreamTypes.AWSS3 ||
                stream.StreamType.Value == DataStreamTypes.AWSElasticSearch)
            {
                if (!String.IsNullOrEmpty(stream.AwsSecretKey))
                {
                    var addSecretResult = await _secureStorage.AddSecretAsync(stream.AwsSecretKey);
                    if (!addSecretResult.Successful) return addSecretResult.ToInvokeResult();
                    stream.AWSSecretKeySecureId = addSecretResult.Result;
                    stream.AwsSecretKey = null;
                }
                else
                {
                    throw new Exception("Validation should have cut null or empty AWSSecretKey, but it did not.");
                }
            }
            else if (stream.StreamType.Value == DataStreamTypes.SQLServer ||
                     stream.StreamType.Value == DataStreamTypes.Postgresql)
            {
                if (!String.IsNullOrEmpty(stream.DbPassword))
                {
                    var addSecretResult = await _secureStorage.AddSecretAsync(stream.DbPassword);
                    if (!addSecretResult.Successful) return addSecretResult.ToInvokeResult();
                    stream.DBPasswordSecureId = addSecretResult.Result;
                    stream.DbPassword = null;
                }
                else
                {
                    throw new Exception("Validation should have cut null or empty DbPassword, but it did not.");
                }
            }
            else
            {
                throw new Exception("New data stream Type was added, should likely add something here to store credentials.");
            }




            await _dataStreamRepo.AddDataStreamAsync(stream);
            return InvokeResult.Success;
        }

        public async Task<InvokeResult<DataStream>> LoadFullDataStreamConfigurationAsync(String id, EntityHeader org, EntityHeader user)
        {
            try
            {
                var stream = await _dataStreamRepo.GetDataStreamAsync(id);

                if (stream.StreamType.Value == DataStreamTypes.AzureBlob ||
                    stream.StreamType.Value == DataStreamTypes.AzureEventHub ||
                    stream.StreamType.Value == DataStreamTypes.AzureTableStorage ||
                    stream.StreamType.Value == DataStreamTypes.AzureTableStorage_Managed)
                {
                    if (String.IsNullOrEmpty(stream.AzureAccessKeySecureId))
                    {
                        return InvokeResult<DataStream>.FromError("Attempt to load an azure type data stream, but secret key id is not present.");
                    }

                    var azureSecretKeyResult = await _secureStorage.GetSecretAsync(stream.AzureAccessKeySecureId, user, org);
                    if (!azureSecretKeyResult.Successful) return InvokeResult<DataStream>.FromInvokeResult(azureSecretKeyResult.ToInvokeResult());

                    stream.AzureAccessKey = azureSecretKeyResult.Result;
                }
                else if (stream.StreamType.Value == DataStreamTypes.AWSS3 ||
                    stream.StreamType.Value == DataStreamTypes.AWSElasticSearch)
                {
                    if (String.IsNullOrEmpty(stream.AWSSecretKeySecureId))
                    {
                        return InvokeResult<DataStream>.FromError("Attempt to load an azure type data stream, but secret key id is not present.");
                    }

                    var awsSecretKeyResult = await _secureStorage.GetSecretAsync(stream.AWSSecretKeySecureId, user, org);
                    if (!awsSecretKeyResult.Successful) return InvokeResult<DataStream>.FromInvokeResult(awsSecretKeyResult.ToInvokeResult());

                    stream.AwsSecretKey = awsSecretKeyResult.Result;
                }
                else if (stream.StreamType.Value == DataStreamTypes.SQLServer ||
                         stream.StreamType.Value == DataStreamTypes.Postgresql)
                {
                    if (String.IsNullOrEmpty(stream.DBPasswordSecureId))
                    {
                        return InvokeResult<DataStream>.FromError("Attempt to load an azure type data stream, but secret key id is not present.");
                    }

                    var dbSecretKeyResult = await _secureStorage.GetSecretAsync(stream.DBPasswordSecureId, user, org);
                    if (!dbSecretKeyResult.Successful) return InvokeResult<DataStream>.FromInvokeResult(dbSecretKeyResult.ToInvokeResult());

                    stream.DbPassword = dbSecretKeyResult.Result;
                }


                return InvokeResult<DataStream>.Create(stream);
            }
            catch (RecordNotFoundException)
            {
                return InvokeResult<DataStream>.FromErrors(ErrorCodes.CouldNotLoadDataStreamModule.ToErrorMessage($"ModuleId={id}"));
            }
        }

        public async Task<DependentObjectCheckResult> CheckDataStreamInUseAsync(string dataStreamId, EntityHeader org, EntityHeader user)
        {
            var dataStream = await _dataStreamRepo.GetDataStreamAsync(dataStreamId);
            await AuthorizeAsync(dataStream, AuthorizeResult.AuthorizeActions.Read, user, org);
            return await CheckForDepenenciesAsync(dataStream);
        }

        public async Task<InvokeResult> DeleteDatStreamAsync(string dataStreamId, EntityHeader org, EntityHeader user)
        {
            var dataStream = await _dataStreamRepo.GetDataStreamAsync(dataStreamId);
            await AuthorizeAsync(dataStream, AuthorizeResult.AuthorizeActions.Delete, user, org);
            await CheckForDepenenciesAsync(dataStream);

            await _dataStreamRepo.DeleteDataStreamAsync(dataStreamId);

            return InvokeResult.Success;
        }

        public async Task<DataStream> GetDataStreamAsync(string dataStreamId, EntityHeader org, EntityHeader user)
        {
            var dataStream = await _dataStreamRepo.GetDataStreamAsync(dataStreamId);
            await AuthorizeAsync(dataStream, AuthorizeResult.AuthorizeActions.Read, user, org);
            return dataStream;
        }

        public async Task<IEnumerable<DataStreamSummary>> GetDataStreamsForOrgAsync(string orgId, EntityHeader user)
        {
            await AuthorizeOrgAccessAsync(user, orgId, typeof(DataStreamSummary));
            return await _dataStreamRepo.GetDataStreamsForOrgAsync(orgId);
        }

        public Task<bool> QueryKeyInUseAsync(string key, EntityHeader org)
        {
            return _dataStreamRepo.QueryKeyInUseAsync(key, org.Id);
        }

        public async Task<InvokeResult> UpdateDataStreamAsync(DataStream stream, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(stream, AuthorizeResult.AuthorizeActions.Update, user, org);
            ValidationCheck(stream, Actions.Update);

            if (stream.StreamType.Value == DataStreamTypes.AzureBlob ||
                stream.StreamType.Value == DataStreamTypes.AzureEventHub ||
                stream.StreamType.Value == DataStreamTypes.AzureTableStorage ||
                stream.StreamType.Value == DataStreamTypes.AzureTableStorage_Managed)
            {
                if (!String.IsNullOrEmpty(stream.AzureAccessKey))
                {
                    var addSecretResult = await _secureStorage.AddSecretAsync(stream.AzureAccessKey);
                    if (!addSecretResult.Successful) return addSecretResult.ToInvokeResult();

                    if (!string.IsNullOrEmpty(stream.AzureAccessKeySecureId))
                    {
                        await _secureStorage.RemoveSecretAsync(stream.AzureAccessKeySecureId);
                    }

                    stream.AzureAccessKeySecureId = addSecretResult.Result;
                    stream.AzureAccessKey = null;
                }
            }
            else if (stream.StreamType.Value == DataStreamTypes.AWSS3 ||
               stream.StreamType.Value == DataStreamTypes.AWSElasticSearch)
            {
                if(!String.IsNullOrEmpty(stream.AwsSecretKey))
                {
                    var addSecretResult = await _secureStorage.AddSecretAsync(stream.AwsSecretKey);
                    if (!addSecretResult.Successful) return addSecretResult.ToInvokeResult();

                    if (!string.IsNullOrEmpty(stream.AWSSecretKeySecureId))
                    {
                        await _secureStorage.RemoveSecretAsync(stream.AWSSecretKeySecureId);
                    }

                    stream.AWSSecretKeySecureId = addSecretResult.Result;
                    stream.AwsSecretKey = null;
                }
            }
            else if(stream.StreamType.Value == DataStreamTypes.SQLServer ||
                    stream.StreamType.Value == DataStreamTypes.Postgresql)
            {
                if (!String.IsNullOrEmpty(stream.DbPassword))
                {
                    var addSecretResult = await _secureStorage.AddSecretAsync(stream.DbPassword);
                    if (!addSecretResult.Successful) return addSecretResult.ToInvokeResult();

                    if (!string.IsNullOrEmpty(stream.DBPasswordSecureId))
                    {
                        await _secureStorage.RemoveSecretAsync(stream.DBPasswordSecureId);
                    }

                    stream.DBPasswordSecureId = addSecretResult.Result;
                    stream.DbPassword = null;
                }
            }

                await _dataStreamRepo.UpdateDataStreamAsync(stream);
            return InvokeResult.Success;
        }

        public async Task<ListResponse<DataStreamResult>> GetStreamDataAsync(DataStream stream, IDataStreamConnector connector, string deviceId,  EntityHeader org, EntityHeader user, ListRequest request)
        {
            await AuthorizeAsync(stream, AuthorizeResult.AuthorizeActions.Read, user, org, "ReadDeviceData");

            await connector.InitAsync(stream);
            return await connector.GetItemsAsync(deviceId, request);
        }
    }
}
