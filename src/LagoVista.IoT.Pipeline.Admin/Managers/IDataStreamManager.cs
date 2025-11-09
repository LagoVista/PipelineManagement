// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 5abc916eda78d8a37c007083856f3ea2a584a644c50f1de941b26f1d7cb2ff79
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Admin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.Admin
{
    public interface IDataStreamManager
    {
        Task<InvokeResult> AddDataStreamAsync(DataStream stream,  EntityHeader org, EntityHeader user);
        Task<InvokeResult> UpdateDataStreamAsync(DataStream stream, EntityHeader org, EntityHeader user);
        Task<InvokeResult<DataStream>> LoadFullDataStreamConfigurationAsync(String id, EntityHeader org, EntityHeader user);
        Task<ListResponse<DataStreamSummary>> GetDataStreamsForOrgAsync(string orgId, EntityHeader user, ListRequest listRequest);
        Task<DataStream> GetDataStreamAsync(string dataStreamId, EntityHeader org, EntityHeader user);
        Task<InvokeResult<string>> GetDataStreamSecretAsync(String id, EntityHeader org, EntityHeader user);
        Task<DependentObjectCheckResult> CheckDataStreamInUseAsync(string dataStreamId, EntityHeader org, EntityHeader user);
        Task<InvokeResult> DeleteDatStreamAsync(string dataStreamId, EntityHeader org, EntityHeader user);
        Task<bool> QueryKeyInUseAsync(string key, EntityHeader org);
        Task<ListResponse<DataStreamResult>> GetStreamDataAsync(DataStream stream, IDataStreamConnector connector, string deviceId, EntityHeader org, EntityHeader user, ListRequest request);
    }
}
