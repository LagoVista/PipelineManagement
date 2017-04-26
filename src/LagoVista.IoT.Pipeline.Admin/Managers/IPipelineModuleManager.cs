using System.Collections.Generic;
using System.Threading.Tasks;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Admin.Models;

namespace LagoVista.IoT.Pipeline.Admin.Managers
{
    public interface IPipelineModuleManager
    {
        Task<InvokeResult> AddInputTranslatorConfigurationAsync(InputTranslatorConfiguration inputTranslatorConfiguration, EntityHeader org, EntityHeader user);
        Task<InvokeResult> AddListenerConfigurationAsync(ListenerConfiguration listenerConfiguration, EntityHeader org, EntityHeader user);
        Task<InvokeResult> AddOutputTranslatorConfigurationAsync(OutputTranslatorConfiguration outputTranslatorConfiguration, EntityHeader org, EntityHeader user);
        Task<InvokeResult> AddCustomPipelineModuleConfigurationAsync(CustomPipelineModuleConfiguration pipelineModuleConfiguration, EntityHeader org, EntityHeader user);
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
        Task<InputTranslatorConfiguration> LoadFullInputTranslatorConfigurationAsync(string id);
        Task<IEnumerable<PipelineModuleConfigurationSummary>> GetInputTranslatorConfiugrationsForOrgAsync(string orgId, EntityHeader user);
        Task<ListenerConfiguration> GetListenerConfigurationAsync(string id, EntityHeader org, EntityHeader user);
        Task<ListenerConfiguration> LoadFullListenerConfigurationAsync(string id);
        Task<IEnumerable<PipelineModuleConfigurationSummary>> GetListenerConfiugrationsForOrgAsync(string orgId, EntityHeader user);
        Task<OutputTranslatorConfiguration> GetOutputTranslatorConfigurationAsync(string id, EntityHeader org, EntityHeader user);
        Task<OutputTranslatorConfiguration> LoadFullOutputTranslatorConfigurationAsync(string id);
        Task<IEnumerable<PipelineModuleConfigurationSummary>> GetOutputTranslatorConfiugrationsForOrgAsync(string orgId, EntityHeader user);
        Task<CustomPipelineModuleConfiguration> GetCustomPipelineModuleConfigurationAsync(string id, EntityHeader org, EntityHeader user);
        Task<CustomPipelineModuleConfiguration> LoadFullCustomPipelineModuleConfigurationAsync(string id);
        Task<IEnumerable<PipelineModuleConfigurationSummary>> GetCustomPipelineModuleConfiugrationsForOrgAsync(string orgId, EntityHeader user);
        Task<SentinelConfiguration> GetSentinelConfigurationAsync(string id, EntityHeader org, EntityHeader user);
        Task<SentinelConfiguration> LoadFullSentinelConfigurationAsync(string id);
        Task<IEnumerable<PipelineModuleConfigurationSummary>> GetSentinelConfiugrationsForOrgAsync(string orgId, EntityHeader user);
        Task<PlannerConfiguration> GetPlannerConfigurationAsync(string id, EntityHeader org, EntityHeader user);
        Task<PlannerConfiguration> LoadFullPlannerConfigurationAsync(string id);
        Task<IEnumerable<PipelineModuleConfigurationSummary>> GetPlannerConfiugrationsForOrgAsync(string orgId, EntityHeader user);
        Task<TransmitterConfiguration> GetTransmitterConfigurationAsync(string id, EntityHeader org, EntityHeader user);
        Task<TransmitterConfiguration> LoadFullTransmitterConfigurationAsync(string id);
        Task<IEnumerable<PipelineModuleConfigurationSummary>> GetTransmitterConfiugrationsForOrgAsync(string orgId, EntityHeader user);
        
        Task<InvokeResult> UpdateInputTranslatorConfigurationAsync(InputTranslatorConfiguration inputTranslatorConfiguration, EntityHeader org, EntityHeader user);
        Task<InvokeResult> UpdateSentinelConfigurationAsync(SentinelConfiguration sentinalConfiguration, EntityHeader org, EntityHeader user);
        Task<InvokeResult> UpdateTransmitterConfigurationAsync(TransmitterConfiguration transmitterConfiguration, EntityHeader org, EntityHeader user);
        Task<InvokeResult> UpdateListenerConfigurationAsync(ListenerConfiguration listenerConfiguration, EntityHeader org, EntityHeader user);
        Task<InvokeResult> UpdateOutputTranslatorConfigurationAsync(OutputTranslatorConfiguration outputTranslatorConfiguration, EntityHeader org, EntityHeader user);
        Task<InvokeResult> UpdatePlannerConfigurationAsync(PlannerConfiguration plannerConfiguration, EntityHeader org, EntityHeader user);
        Task<InvokeResult> UpdateCustomPipelineModuleConfigurationAsync(CustomPipelineModuleConfiguration pipelineModuleConfiguration, EntityHeader org, EntityHeader user);
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
    }
}