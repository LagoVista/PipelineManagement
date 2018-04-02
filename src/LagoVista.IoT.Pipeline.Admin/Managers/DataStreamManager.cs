using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LagoVista.Core.Exceptions;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Managers;
using LagoVista.Core.Models;
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

        public DataStreamManager(IDataStreamRepo dataStreamRepo, IAdminLogger logger, ISecureStorage secureStorage,IAppConfig appConfig, IDependencyManager depmanager, ISecurity security) :
            base(logger, appConfig, depmanager, security)
        {
            _dataStreamRepo = dataStreamRepo;
            _secureStorage = secureStorage;
        }

        public async Task<InvokeResult> AddDataStreamAsync(DataStream stream, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(stream, AuthorizeResult.AuthorizeActions.Create, user, org);
            ValidationCheck(stream, Actions.Create);

            if (!String.IsNullOrEmpty(stream.AzureAccessKey))
            {
                var addSecretResult = await _secureStorage.AddSecretAsync(stream.AzureAccessKey);
                if (!addSecretResult.Successful) return addSecretResult.ToInvokeResult();
                stream.AzureAccessKeySecureId = addSecretResult.Result;
                stream.AzureAccessKey = null;
            }

            await _dataStreamRepo.AddDataStreamAsync(stream);
            return InvokeResult.Success;
        }

        public async Task<InvokeResult<DataStream>> LoadFullDataStreamConfigurationAsync(String id)
        {
            try
            {
                return InvokeResult<DataStream>.Create(await _dataStreamRepo.GetDataStreamAsync(id));
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

            await _dataStreamRepo.UpdateDataStreamAsync(stream);
            return InvokeResult.Success;
        }
    }
}
