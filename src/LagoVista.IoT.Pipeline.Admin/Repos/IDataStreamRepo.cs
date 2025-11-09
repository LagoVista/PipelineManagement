// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: d82ae623bb05ec911d2f9f1e025324a56221770ad203e9632fef4bd1eadc6c4f
// IndexVersion: 2
// --- END CODE INDEX META ---
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
