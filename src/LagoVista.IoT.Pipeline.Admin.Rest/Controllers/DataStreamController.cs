using LagoVista.Core;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using LagoVista.IoT.DataStreamConnectors;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Web.Common.Controllers;
using LagoVista.UserAdmin.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.Admin.Rest.Controllers
{
    /// <summary>
    /// Device Stream Controller
    /// </summary>
    [Authorize]
    public class DataStreamController : LagoVistaBaseController
    {
        IDataStreamManager _dataStreamManager;
        IAdminLogger _adminlogger;

        public DataStreamController(IDataStreamManager dataStreamManager, UserManager<AppUser> userManager,  IAdminLogger logger) : base(userManager, logger)
        {
            _dataStreamManager = dataStreamManager;
            _adminlogger = logger;
        }

        /// <summary>
        /// Data Stream - Add
        /// </summary>
        /// <param name="datastream"></param>
        /// <returns></returns>
        [HttpPost("/api/datastream")]
        public Task<InvokeResult> AddHostAsync([FromBody] DataStream datastream)
        {
            return _dataStreamManager.AddDataStreamAsync(datastream, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Data Stream - Get
        /// </summary>
        /// <returns>A Data Stream</returns>
        [HttpGet("/api/datastream/{id}")]
        public async Task<DetailResponse<DataStream>> GetDataStreamAsync(string id)
        {
            var dataStream = await _dataStreamManager.GetDataStreamAsync(id, OrgEntityHeader, UserEntityHeader);
            return DetailResponse<DataStream>.Create(dataStream);
        }

        /// <summary>
        /// Data Stream - Update
        /// </summary>
        /// <param name="datastream"></param>
        /// <returns></returns>
        [HttpPut("/api/datastream")]
        public Task<InvokeResult> UpdateDataStreamAsync([FromBody] DataStream datastream)
        {
            SetUpdatedProperties(datastream);
            return _dataStreamManager.UpdateDataStreamAsync(datastream, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Data Stream - Get for Current Org
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/datastreams")]
        public async Task<ListResponse<DataStreamSummary>> GetDataStreamsForOrgAsync()
        {
            var hostSummaries = await _dataStreamManager.GetDataStreamsForOrgAsync(OrgEntityHeader.Id, UserEntityHeader);
            var response = ListResponse<DataStreamSummary>.Create(hostSummaries);

            return response;
        }

        /// <summary>
        /// Data Stream - Can Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/datastream/{id}/inuse")]
        public Task<DependentObjectCheckResult> InUseCheck(String id)
        {
            return _dataStreamManager.CheckDataStreamInUseAsync(id, OrgEntityHeader, UserEntityHeader);
        }
        
        /// <summary>
        /// Data Stream - Key In Use
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/datastream/{key}/keyinuse")]
        public Task<bool> HostKeyInUse(String key)
        {
            return _dataStreamManager.QueryKeyInUseAsync(key, OrgEntityHeader);
        }

        /// <summary>
        /// Data Stream - Delete
        /// </summary>
        /// <returns></returns>
        [HttpDelete("/api/datastream/{id}")]
        public Task<InvokeResult> DeleteDataStreamAsync(string id)
        {
            return _dataStreamManager.DeleteDatStreamAsync(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Data Stream - Validate connection by id
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/datastream/{id}/testconnection")]
        public async Task<InvokeResult> ValidateDataStreamAsync(string id)
        {
            var stream = await _dataStreamManager.GetDataStreamAsync(id, OrgEntityHeader, UserEntityHeader);
            return await DataStreamValidator.ValidateDataStreamAsync(stream, _adminlogger);
        }

        /// <summary>
        /// Data Stream - Validate by post
        /// </summary>
        /// <returns></returns>
        [HttpPost("/api/datastream/testconnection")]
        public async Task<InvokeResult> ValidateDataStreamAsync([FromBody] DataStream stream)
        {
            return await DataStreamValidator.ValidateDataStreamAsync(stream, _adminlogger);
        }

        /// <summary>
        /// Data Stream - Get Data for Device
        /// </summary>
        /// <param name="datastreamid"></param>
        /// <param name="deviceid"></param>
        /// <returns></returns>
        [HttpGet("/api/datastream/{datastreamid}/data/{deviceid}")]
        public async Task<ListResponse<DataStreamResult>> GetDataAsync(string datastreamid, string deviceid)
        {
            var dataStream = await _dataStreamManager.GetDataStreamAsync(datastreamid, OrgEntityHeader, UserEntityHeader);

            var connectorResult = DataStreamServices.GetConnector(dataStream.StreamType.Value, _adminlogger);

            return await _dataStreamManager.GetStreamDataAsync(dataStream, connectorResult.Result, deviceid, OrgEntityHeader, UserEntityHeader, GetListRequestFromHeader());
        }

        /// <summary>
        /// Data Stream - Create New
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/datastream/factory")]
        public DetailResponse<DataStream> CreateDataStream()
        {

            var response = DetailResponse<DataStream>.Create();
            response.Model.Id = Guid.NewGuid().ToId();
            SetAuditProperties(response.Model);
            SetOwnedProperties(response.Model);

            return response;
        }

        /// <summary>
        /// Data Stream - Create New Field
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/datastreamfield/factory")]
        public DetailResponse<DataStreamField> CreateDataStreamField()
        {
            var field = DetailResponse<DataStreamField>.Create();
            field.Model.Id = Guid.NewGuid().ToId();
            return field;
        }        
    }
}
