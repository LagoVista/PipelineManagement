﻿using LagoVista.Core;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Models;
using LagoVista.IoT.Web.Common.Attributes;
using LagoVista.IoT.Web.Common.Controllers;
using LagoVista.UserAdmin.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.Admin.Rest.Controllers
{
    /// <summary>
    /// Device Stream Controller
    /// </summary>
    [Authorize]
    [AppBuilder]
    public class ApplicationCacheController : LagoVistaBaseController
    {
        private readonly IApplicationCacheManager _applicationCacheManager;
      
        public ApplicationCacheController(IApplicationCacheManager applicationCacheManager, UserManager<AppUser> userManager, IAdminLogger logger) : base(userManager, logger)
        {
            _applicationCacheManager = applicationCacheManager ?? throw new ArgumentNullException(nameof(applicationCacheManager));
        }

        /// <summary>
        /// Application Cache - Add
        /// </summary>
        /// <param name="applicationcache"></param>
        /// <returns></returns>
        [HttpPost("/api/appcache")]
        public Task<InvokeResult> AddHostAsync([FromBody] ApplicationCache applicationcache)
        {
            return _applicationCacheManager.AddApplicationCacheAsync(applicationcache, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Application Cache - Get
        /// </summary>
        /// <returns>A Application Cache</returns>
        [HttpGet("/api/appcache/{id}")]
        public async Task<DetailResponse<ApplicationCache>> GetApplicationCacheAsync(string id)
        {
            var dataStream = await _applicationCacheManager.GetApplicationCacheAsync(id, OrgEntityHeader, UserEntityHeader);
            return DetailResponse<ApplicationCache>.Create(dataStream);
        }

        /// <summary>
        /// Application Cache - Update
        /// </summary>
        /// <param name="applicationcache"></param>
        /// <returns></returns>
        [HttpPut("/api/appcache")]
        public Task<InvokeResult> UpdateApplicationCacheAsync([FromBody] ApplicationCache applicationcache)
        {
            SetUpdatedProperties(applicationcache);
            return _applicationCacheManager.UpdateApplicationCacheAsync(applicationcache, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Application Cache - Get for Current Org
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/appcaches")]
        public Task<ListResponse<ApplicationCacheSummary>> GetApplicationCachesForOrgAsync()
        {
            return _applicationCacheManager.GetApplicationCachesForOrgAsync(OrgEntityHeader.Id, UserEntityHeader, GetListRequestFromHeader());
        }

        /// <summary>
        /// Application Cache - Can Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/appcache/{id}/inuse")]
        public Task<DependentObjectCheckResult> InUseCheck(String id)
        {
            return _applicationCacheManager.CheckApplicationCacheInUseAsync(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Application Cache - Key In Use
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/appcache/{key}/keyinuse")]
        public Task<bool> HostKeyInUse(String key)
        {
            return _applicationCacheManager.QueryKeyInUseAsync(key, OrgEntityHeader);
        }

        /// <summary>
        /// Application Cache - Delete
        /// </summary>
        /// <returns></returns>
        [HttpDelete("/api/appcache/{id}")]
        public Task<InvokeResult> DeleteApplicationCacheAsync(string id)
        {
            return _applicationCacheManager.DeleteDatStreamAsync(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Application Cache - Create New
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/appcache/factory")]
        public DetailResponse<ApplicationCache> CreateApplicationCache()
        {

            var response = DetailResponse<ApplicationCache>.Create();
            response.Model.Id = Guid.NewGuid().ToId();
            SetAuditProperties(response.Model);
            SetOwnedProperties(response.Model);
            return response;
        }

        /// <summary>
        /// Application Cache Value - Create New
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/appcache/value/factory")]
        public DetailResponse<ApplicationCacheValue> CreateApplicationCacheValue()
        {
            var value = DetailResponse<ApplicationCacheValue>.Create();
            value.Model.CreationDate = DateTime.UtcNow.ToJSONString();
            value.Model.LastUpdateDate = value.Model.CreationDate;
            return value;
        }
    }
}
