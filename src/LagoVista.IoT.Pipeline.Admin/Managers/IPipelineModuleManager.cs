using System.Collections.Generic;
using System.Threading.Tasks;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Admin.Models;

namespace LagoVista.IoT.Pipeline.Admin.Managers
{
    public interface IPipelineModuleManager
    {
        Task<InvokeResult> AddInputTranslatorConfigurationAsync(InputTranslatorConfiguration inputTranslatorConfiguration, EntityHeader org, EntityHeader user);
        Task<InvokeResult> AddListenerConfigurationAsync(ListenerConfiguration listenerConfiguration, EntityHeader org, EntityHeader user);
        Task<InvokeResult> AddOutputTranslatorConfigurationAsync(OutputTranslatorConfiguration outputTranslatorConfiguration, EntityHeader org, EntityHeader user);
        Task<InvokeResult> AddCustomPipelineModuleConfigurationAsync(CustomModuleConfiguration pipelineModuleConfiguration, EntityHeader org, EntityHeader user);
        Task<InvokeResult> AddSentinelConfigurationAsync(SentinelConfiguration sentinelConfiguration, EntityHeader org, EntityHeader user);
        Task<InvokeResult> AddTransmitterConfigurationAsync(TransmitterConfiguration transmitterConfiguration, EntityHeader org, EntityHeader user);
        Task<InvokeResult> AddPlannerConfigurationAsync(PlannerConfiguration plannerConfiguration, EntityHeader org, EntityHeader user);
        Task<InvokeResult> DeleteInputTranslatorAsync(string id, EntityHeader org, EntityHeader user);
        Task<InvokeResult> DeleteListenerAsync(string id, EntityHeader org, EntityHeader user);
        Task<InvokeResult> DeleteOutputTranslatorAsync(string id, EntityHeader org, EntityHeader user);
        Task<InvokeResult> DeleteCustomPipelineModuleAsync(string id, EntityHeader org, EntityHeader user);
        Task<InvokeResult> DeleteSentinelAsync(string id, EntityHeader org, EntityHeader user);
        Task<InvokeResult> DeletePlannerAsync(string id, EntityHeader org, EntityHeader user);
        Task<InvokeResult> DeleteTransmitterAsync(string id, EntityHeader org, EntityHeader user);
        Task<InputTranslatorConfiguration> GetInputTranslatorConfigurationAsync(string id, EntityHeader org, EntityHeader user);
        Task<InvokeResult<InputTranslatorConfiguration>> LoadFullInputTranslatorConfigurationAsync(string id);
        Task<ListResponse<InputTranslatorConfigurationSummary>> GetInputTranslatorConfiugrationsForOrgAsync(string orgId, EntityHeader user, ListRequest listRequest);
        Task<ListenerConfiguration> GetListenerConfigurationAsync(string id, EntityHeader org, EntityHeader user);
        Task<InvokeResult<ListenerConfiguration>> LoadFullListenerConfigurationAsync(string id);
        Task<ListResponse<ListenerConfigurationSummary>> GetListenerConfiugrationsForOrgAsync(string orgId, EntityHeader user, ListRequest listRequest);
        Task<OutputTranslatorConfiguration> GetOutputTranslatorConfigurationAsync(string id, EntityHeader org, EntityHeader user);
        Task<InvokeResult<OutputTranslatorConfiguration>> LoadFullOutputTranslatorConfigurationAsync(string id);
        Task<ListResponse<OutputTranslatorConfigurationSummary>> GetOutputTranslatorConfiugrationsForOrgAsync(string orgId, EntityHeader user, ListRequest listRequest);
        Task<CustomModuleConfiguration> GetCustomPipelineModuleConfigurationAsync(string id, EntityHeader org, EntityHeader user);
        Task<InvokeResult<CustomModuleConfiguration>> LoadFullCustomPipelineModuleConfigurationAsync(string id);
        Task<ListResponse<CustomModuleConfigurationSummary>> GetCustomPipelineModuleConfiugrationsForOrgAsync(string orgId, EntityHeader user, ListRequest listRequest);
        Task<SentinelConfiguration> GetSentinelConfigurationAsync(string id, EntityHeader org, EntityHeader user);
        Task<InvokeResult<SentinelConfiguration>> LoadFullSentinelConfigurationAsync(string id);
        Task<ListResponse<SentinelConfigurationSummary>> GetSentinelConfiugrationsForOrgAsync(string orgId, EntityHeader user, ListRequest listRequest);
        Task<PlannerConfiguration> GetPlannerConfigurationAsync(string id, EntityHeader org, EntityHeader user);
        Task<InvokeResult<PlannerConfiguration>> LoadFullPlannerConfigurationAsync(string id);
        Task<ListResponse<PlannerConfigurationSummary>> GetPlannerConfiugrationsForOrgAsync(string orgId, EntityHeader user, ListRequest listRequest);
        Task<TransmitterConfiguration> GetTransmitterConfigurationAsync(string id, EntityHeader org, EntityHeader user);
        Task<InvokeResult<TransmitterConfiguration>> LoadFullTransmitterConfigurationAsync(string id);
        Task<ListResponse<TransmitterConfigurationSummary>> GetTransmitterConfiugrationsForOrgAsync(string orgId, EntityHeader user, ListRequest listRequest);
        
        Task<InvokeResult> UpdateInputTranslatorConfigurationAsync(InputTranslatorConfiguration inputTranslatorConfiguration, EntityHeader org, EntityHeader user);
        Task<InvokeResult> UpdateSentinelConfigurationAsync(SentinelConfiguration sentinalConfiguration, EntityHeader org, EntityHeader user);
        Task<InvokeResult> UpdateTransmitterConfigurationAsync(TransmitterConfiguration transmitterConfiguration, EntityHeader org, EntityHeader user);
        Task<InvokeResult> UpdateListenerConfigurationAsync(ListenerConfiguration listenerConfiguration, EntityHeader org, EntityHeader user);
        Task<InvokeResult> UpdateOutputTranslatorConfigurationAsync(OutputTranslatorConfiguration outputTranslatorConfiguration, EntityHeader org, EntityHeader user);
        Task<InvokeResult> UpdatePlannerConfigurationAsync(PlannerConfiguration plannerConfiguration, EntityHeader org, EntityHeader user);
        Task<InvokeResult> UpdateCustomPipelineModuleConfigurationAsync(CustomModuleConfiguration pipelineModuleConfiguration, EntityHeader org, EntityHeader user);
        Task<bool> QueryKeyInUsePlannerConfigurationAsync(string key, EntityHeader org);
        Task<bool> QueryKeyInUseListenerConfigurationAsync(string key, EntityHeader org);
        Task<bool> QueryKeyInUseInputTranslatorConfigurationAsync(string key, EntityHeader org);
        Task<bool> QueryKeyInUseSentinelConfigurationAsync(string key, EntityHeader org);
        Task<bool> QueryKeyInUseOutputTranslatorConfigurationAsync(string key, EntityHeader org);
        Task<bool> QueryKeyInUseTransmitterConfigurationAsync(string key, EntityHeader org);
        Task<bool> QueryKeyInUseCustomPipelineModuleConfigurationAsync(string key, EntityHeader org);
        Task<DependentObjectCheckResult> CheckInUseInputTranslator(string id, EntityHeader org, EntityHeader user);
        Task<DependentObjectCheckResult> CheckInUseListener(string id, EntityHeader org, EntityHeader user);
        Task<DependentObjectCheckResult> CheckInUseSentinel(string id, EntityHeader org, EntityHeader user);
        Task<DependentObjectCheckResult> CheckInUseOutputTranslator(string id, EntityHeader org, EntityHeader user);
        Task<DependentObjectCheckResult> CheckInUseTransmitter(string id, EntityHeader org, EntityHeader user);
        Task<DependentObjectCheckResult> CheckInUsePlanner(string id, EntityHeader org, EntityHeader user);
        Task<DependentObjectCheckResult> CheckInUseCustom(string id, EntityHeader org, EntityHeader user);
        bool IsForInitialization { get; set; }
    }
}