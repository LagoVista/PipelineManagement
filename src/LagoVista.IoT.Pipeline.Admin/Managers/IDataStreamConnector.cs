using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Admin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.Admin
{
    public interface IDataStreamConnector
    {
        Task<InvokeResult> AddItemAsync(DataStream stream, DataStreamRecord item, EntityHeader org, EntityHeader user);
        Task<InvokeResult> AddItemAsync(DataStream stream, DataStreamRecord item);
        Task<ListResponse<List<DataStreamRecord>>> GetItemsAsync(DataStream stream, string deviceId, ListRequest request, EntityHeader org, EntityHeader user);
    }
}
