using LagoVista.Core.Models.UIMetaData;
using LagoVista.IoT.Pipeline.Admin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.Admin.Repos
{
    public interface IDataStreamRepo
    {
        Task AddDataStreamAsync(DataStream dataStream);

        Task UpdateDataStreamAsync(DataStream dataStream);

        Task<DataStream> GetDataStreamAsync(string id);

        Task<ListResponse<DataStreamSummary>> GetDataStreamsForOrgAsync(string orgId, ListRequest listRequest);

        Task DeleteDataStreamAsync(string dataStreamId);

        Task<bool> QueryKeyInUseAsync(string key, string orgId);
    }
}
