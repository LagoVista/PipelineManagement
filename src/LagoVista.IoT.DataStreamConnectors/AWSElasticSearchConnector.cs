using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Admin;
using LagoVista.IoT.Pipeline.Admin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.DataStreamConnectors
{
    public class AWSElasticSearchConnector : IDataStreamConnector
    {
        DataStream _stream;

        public Task<InvokeResult> InitAsync(DataStream stream)
        {
            _stream = stream;
            throw new NotImplementedException();
        }

        public Task<InvokeResult> AddItemAsync(DataStreamRecord item, LagoVista.Core.Models.EntityHeader org, LagoVista.Core.Models.EntityHeader user)
        {
            throw new NotImplementedException();
        }

        public Task<InvokeResult> AddItemAsync(DataStreamRecord item)
        {
            throw new NotImplementedException();
        }

        public Task<LagoVista.Core.Models.UIMetaData.ListResponse<List<DataStreamRecord>>> GetItemsAsync(string deviceId, LagoVista.Core.Models.UIMetaData.ListRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
