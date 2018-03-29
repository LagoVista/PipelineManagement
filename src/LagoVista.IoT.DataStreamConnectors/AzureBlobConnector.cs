using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Admin;
using LagoVista.IoT.Pipeline.Admin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.DataStreamConnectors
{
    public class AzureBlobConnector : IDataStreamConnector
    {
        public Task<InvokeResult> AddItemAsync(DataStream stream, DataStreamRecord archiveEntry, LagoVista.Core.Models.EntityHeader org, LagoVista.Core.Models.EntityHeader user)
        {
            throw new NotImplementedException();
        }

        public Task<InvokeResult> AddItemAsync(DataStream stream, DataStreamRecord archiveEntry)
        {
            throw new NotImplementedException();
        }

        public Task<LagoVista.Core.Models.UIMetaData.ListResponse<List<DataStreamRecord>>> GetItemsAsync(DataStream stream, string deviceId, LagoVista.Core.Models.UIMetaData.ListRequest request, LagoVista.Core.Models.EntityHeader org, LagoVista.Core.Models.EntityHeader user)
        {
            throw new NotImplementedException();
        }

    }
}
