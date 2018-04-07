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
        Task<InvokeResult<DataStream>> LoadFullDataStreamConfigurationAsync(String id);
        Task<IEnumerable<DataStreamSummary>> GetDataStreamsForOrgAsync(string orgId, EntityHeader user);

        Task<DataStream> GetDataStreamAsync(string dataStreamId, EntityHeader org, EntityHeader user);

        Task<DependentObjectCheckResult> CheckDataStreamInUseAsync(string dataStreamId, EntityHeader org, EntityHeader user);

        Task<InvokeResult> DeleteDatStreamAsync(string dataStreamId, EntityHeader org, EntityHeader user);

        Task<bool> QueryKeyInUseAsync(string key, EntityHeader org);
        Task<ListResponse<DataStreamResult>> GetStreamDataAsync(DataStream stream, IDataStreamConnector connector, string deviceId, EntityHeader org, EntityHeader user, ListRequest request);
    }
}
