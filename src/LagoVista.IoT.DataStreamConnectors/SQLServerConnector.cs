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
        string _connectionString;

        public class SQLFieldMetaData
        {
            public string ColumnName { get; set; }
            public Boolean IsRequired { get; set; }
            public string DataType { get; set; }
            public int? MaxLength { get; set; }
        }

        public SQLServerConnector(Logging.Loggers.IInstanceLogger instanceLogger)
        {
            _instanceLogger = instanceLogger;
        }

        public async Task<ValidationResult> ValidationConnection(DataStream stream)
        {
            var result = new ValidationResult();

            /* be careful when updating the SQL below, the rdr uses field indexes,
             * if this wasn't so small and self contained, I probably wouldn't be so lazy,
             * buf for one field...well...moving on.*/
            var sql = $@"
SELECT column_name, IS_NULLABLE, data_type, character_maximum_length
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = N'{stream.DBTableName}'";

            var fields = new List<SQLFieldMetaData>();

            using (var cn = new System.Data.SqlClient.SqlConnection(_connectionString))
            using (var cmd = new System.Data.SqlClient.SqlCommand(sql, cn))
            {
                await cn.OpenAsync();
                using (var rdr = await cmd.ExecuteReaderAsync())
                {
                    while (await rdr.ReadAsync())
                    {
                        fields.Add(new SQLFieldMetaData()
                        {
                            ColumnName = rdr["column_name"].ToString(),
                            IsRequired = rdr["IS_NULLABLE"].ToString() == "NO",
                            DataType = rdr["data_type"].ToString(),
                            MaxLength = rdr.IsDBNull(3) ? (Int32?)null : Convert.ToInt32(rdr["character_maximum_length"])
                        });
                    }
                }
            }

            return result;
        }

        public async Task<InvokeResult> InitAsync(DataStream stream)
        {
            _stream = stream;

            var builder = new System.Data.SqlClient.SqlConnectionStringBuilder();
            builder.Add("Data Source", stream.DBURL);
            builder.Add("Initial Catalog", stream.DBName);
            builder.Add("User Id", stream.DBUserName);
            builder.Add("Password", stream.DBPassword);
            _connectionString = builder.ConnectionString;

            if (stream.DBValidateSchema)
            {
                var result = await ValidationConnection(stream);
                if(!result.Successful)
                {
                    return result.ToInvokeResult();
                }
            }

            return InvokeResult.Success;
        }

        public async Task<InvokeResult> AddItemAsync(DataStreamRecord item, LagoVista.Core.Models.EntityHeader org, LagoVista.Core.Models.EntityHeader user)
        {
            var fields = String.Empty;
            var values = String.Empty;
            foreach (var fld in _stream.Fields)
            {
                /* validation should happen long before this point, however if someone manipulated the value, it could be very, very bad
                 * with a SQL injection attack, so error on the side of caution and never let it get through.
                 */
                if(!Validator.Validate(fld).Successful) throw new Exception($"Invalid field name {fld.FieldName}");

                fields += String.IsNullOrEmpty(fields) ? $"{fld.FieldName}" : $",{fld.FieldName}";
                values += String.IsNullOrEmpty(fields) ? $"@{fld.FieldName}" : $",@{fld.FieldName}";
            }

            var sql = $"insert into [{_stream.DBTableName}] ({fields}) values ({values})";

            using (var cn = new System.Data.SqlClient.SqlConnection(_connectionString))
            using (var cmd = new System.Data.SqlClient.SqlCommand(sql, cn))
            {
                cmd.CommandType = System.Data.CommandType.Text;
                await cn.OpenAsync();
                var insertResult = await cmd.ExecuteNonQueryAsync();
            }

            return InvokeResult.Success;

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

