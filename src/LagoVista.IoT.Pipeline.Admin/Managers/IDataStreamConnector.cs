using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.IoT.Pipeline.Admin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.Admin
{
    public interface IDataStreamConnector
    {
        Task AddItemAsync(DataStream stream, DataStreamRecord item, EntityHeader org, EntityHeader user);
        Task AddItemAsync(DataStream stream, DataStreamRecord item);
        Task<ListResponse<List<DataStreamRecord>>> GetItemsAsync(DataStream stream, string deviceId, ListRequest request, EntityHeader org, EntityHeader user);
    }
}
