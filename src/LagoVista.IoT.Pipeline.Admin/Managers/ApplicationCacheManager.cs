// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 3dd31bbc8f3ae0d6319b5169e8a528d0ca37d709f15940b0f6eb806d43fe9484
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Exceptions;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Managers;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Pipeline.Admin.Repos;
using LagoVista.IoT.Pipeline.Admin.Resources;
using LagoVista.IoT.Pipeline.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.Admin.Managers
{
    public class ApplicationCacheManager : ManagerBase, IApplicationCacheManager
    {
        IApplicationCacheRepo _applicationCacheRepo;
        ISecureStorage _secureStorage;

        public ApplicationCacheManager(IApplicationCacheRepo applicationCacheRepo, IAdminLogger logger, ISecureStorage secureStorage, 
            IAppConfig appConfig, IDependencyManager depmanager, ISecurity security) : base(logger, appConfig, depmanager, security)
        {
            _applicationCacheRepo = applicationCacheRepo;
            _secureStorage = secureStorage;
        }

        public async Task<InvokeResult> AddApplicationCacheAsync(ApplicationCache cache, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(cache, AuthorizeResult.AuthorizeActions.Create, user, org);

            ValidationCheck(cache, Actions.Create);

            if (cache.CacheType.Value == CacheTypes.Redis && !String.IsNullOrEmpty(cache.Password))
            {
                var addSecretResult = await _secureStorage.AddSecretAsync(org, cache.Password);
                if (!addSecretResult.Successful)
                {
                    return addSecretResult.ToInvokeResult();
                }

                cache.PasswordSecretId = addSecretResult.Result;
                cache.Password = null;
            }

            await _applicationCacheRepo.AddApplicationCacheAsync(cache);
            return InvokeResult.Success;
        }

        public async Task<InvokeResult<ApplicationCache>> LoadFullApplicationCacheConfigurationAsync(String id, EntityHeader org, EntityHeader user)
        {
            try
            {
                var cache = await _applicationCacheRepo.GetApplicationCacheAsync(id);

                await AuthorizeAsync(cache, AuthorizeResult.AuthorizeActions.Read, user, org, "LoadFull");

                if (cache.CacheType.Value == CacheTypes.Redis && !String.IsNullOrEmpty(cache.PasswordSecretId))
                {
                    var getResult = await _secureStorage.GetSecretAsync(org, cache.PasswordSecretId, user);
                    if (!getResult.Successful)
                    {
                        return InvokeResult<ApplicationCache>.FromInvokeResult(getResult.ToInvokeResult());
                    }

                    cache.Password = getResult.Result;
                }

                return InvokeResult<ApplicationCache>.Create(cache);
            }
            catch (RecordNotFoundException)
            {
                return InvokeResult<ApplicationCache>.FromErrors(ErrorCodes.CouldNotLoadDataStreamModule.ToErrorMessage($"ModuleId={id}"));
            }
        }

        public async Task<DependentObjectCheckResult> CheckApplicationCacheInUseAsync(string ApplicationCacheId, EntityHeader org, EntityHeader user)
        {
            var cache = await _applicationCacheRepo.GetApplicationCacheAsync(ApplicationCacheId);
            await AuthorizeAsync(cache, AuthorizeResult.AuthorizeActions.Read, user, org);
            return await CheckForDepenenciesAsync(cache);
        }

        public async Task<InvokeResult> DeleteDatStreamAsync(string ApplicationCacheId, EntityHeader org, EntityHeader user)
        {
            var cache = await _applicationCacheRepo.GetApplicationCacheAsync(ApplicationCacheId);
            await AuthorizeAsync(cache, AuthorizeResult.AuthorizeActions.Delete, user, org);
            await CheckForDepenenciesAsync(cache);

            await _applicationCacheRepo.DeleteApplicationCacheAsync(ApplicationCacheId);

            return InvokeResult.Success;
        }

        public async Task<ApplicationCache> GetApplicationCacheAsync(string ApplicationCacheId, EntityHeader org, EntityHeader user)
        {
            var cahce = await _applicationCacheRepo.GetApplicationCacheAsync(ApplicationCacheId);
            await AuthorizeAsync(cahce, AuthorizeResult.AuthorizeActions.Read, user, org);
            return cahce;
        }

        public async Task<ListResponse<ApplicationCacheSummary>> GetApplicationCachesForOrgAsync(string orgId, EntityHeader user, ListRequest request)
        {
            await AuthorizeOrgAccessAsync(user, orgId, typeof(ApplicationCache));
            return await _applicationCacheRepo.GetApplicationCachesForOrgAsync(orgId, request);
        }

        public Task<bool> QueryKeyInUseAsync(string key, EntityHeader org)
        {
            return _applicationCacheRepo.QueryKeyInUseAsync(key, org.Id);
        }

        public async Task<InvokeResult> UpdateApplicationCacheAsync(ApplicationCache cache, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(cache, AuthorizeResult.AuthorizeActions.Update, user, org);
            ValidationCheck(cache, Actions.Update);

            if (cache.CacheType.Value == CacheTypes.Redis)
            {
                if (!String.IsNullOrEmpty(cache.Password))
                {
                    var addSecretResult = await _secureStorage.AddSecretAsync(org, cache.Password);
                    if (!addSecretResult.Successful)
                    {
                        return addSecretResult.ToInvokeResult();
                    }

                    if (!string.IsNullOrEmpty(cache.PasswordSecretId))
                    {
                        await _secureStorage.RemoveSecretAsync(org, cache.PasswordSecretId);
                    }

                    cache.PasswordSecretId = addSecretResult.Result;
                    cache.Password = null;
                }
            }

            await _applicationCacheRepo.UpdateApplicationCacheAsync(cache);
            return InvokeResult.Success;
        }

    }
}
