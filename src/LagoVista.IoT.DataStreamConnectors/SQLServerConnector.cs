using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Admin;
using LagoVista.IoT.Pipeline.Admin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.DataStreamConnectors
{
    public class SQLServerConnector : IDataStreamConnector
    {
        DataStream _stream;
        Logging.Loggers.IInstanceLogger _instanceLogger;

        public SQLServerConnector(Logging.Loggers.IInstanceLogger instanceLogger)
        {
            _instanceLogger = instanceLogger;
        }

        public async Task<InvokeResult> InitAsync(DataStream stream)
        {
            _stream = stream;

            var builder = new System.Data.SqlClient.SqlConnectionStringBuilder();
            builder.Add("Data Source", stream.DBURL);
            builder.Add("Initial Catalog", stream.DBName);
            builder.Add("User Id", stream.DBUserName);
            builder.Add("Password", stream.DBPassword);

            using (var cn = new System.Data.SqlClient.SqlConnection(builder.ConnectionString))
            using (var cmd = new System.Data.SqlClient.SqlCommand($"select * from [{stream.DBTableName}]", cn)) 
            {
                await cn.OpenAsync();
                using (var rdr = cmd.ExecuteReader())
                {
                    while(rdr.Read())
                    {
                        Console.WriteLine(rdr["deviceid"]);
                    }
                }

            }

            return InvokeResult.Success;

        }

        public Task<InvokeResult> AddItemAsync(DataStreamRecord item, LagoVista.Core.Models.EntityHeader org, LagoVista.Core.Models.EntityHeader user)
        {
            throw new NotImplementedException();
        }

        public Task<InvokeResult> AddItemAsync(DataStreamRecord item)
        {
            throw new NotImplementedException();
        }

        public async Task<LagoVista.Core.Models.UIMetaData.ListResponse<DataStreamResult>> GetItemsAsync(string deviceId, LagoVista.Core.Models.UIMetaData.ListRequest request)
        {
            throw new NotImplementedException();
        }
    }
}

