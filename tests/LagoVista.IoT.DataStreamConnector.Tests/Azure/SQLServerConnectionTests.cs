using LagoVista.IoT.Pipeline.Admin.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using LagoVista.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.DataStreamConnector.Tests.Azure
{
    /* 
     * To run these tests ensure that the SQL Server parameters as described below are environment variables
     * The database should also have a table named "unittest" with the following schema
     
CCREATE TABLE [dbo].[unittest](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[deviceId] [varchar](128) NOT NULL,
	[timeStamp] [datetime] NOT NULL,
	[value1] [int] NOT NULL,
	[value2] [int] NULL,
	[value3] [float] NULL,
	[location] [geography] NULL,
 CONSTRAINT [PK__unittest__3214EC070EB089E8] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)
        
       * */

    [TestClass]
    public class SQLServerConnectionTests : DataStreamConnectorTestBase
    {
        DataStream _stream;
        private DataStream GetValidStream()
        {
            if (_stream != null)
            {
                return _stream;
            }

            _stream = new Pipeline.Admin.Models.DataStream()
            {
                Id = "06A0754DB67945E7BAD5614B097C61F5",
                StreamType = Core.Models.EntityHeader<DataStreamTypes>.Create(DataStreamTypes.SQLServer),
                DBUserName = System.Environment.GetEnvironmentVariable("SQLSERVERUID"),
                DBName = System.Environment.GetEnvironmentVariable("SQLSERVERDB"),
                DBURL = System.Environment.GetEnvironmentVariable("SQLSERVERURL"),
                DBPassword = System.Environment.GetEnvironmentVariable("SQLSERVERPWD"),
                AzureAccessKey = System.Environment.GetEnvironmentVariable("AZUREACCESSKEY"),
                DBTableName = "unittest"
            };

            return _stream;
        }

        [TestMethod]
        public async Task SQLServer_Init()
        {
            var stream = GetValidStream();

            var connector = new DataStreamConnectors.SQLServerConnector(new Logging.Loggers.InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID"));
            Assert.IsTrue((await connector.InitAsync(stream)).Successful);
        }

    }
}
