// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: bfee15dea1a7480fa2058e3b700ee3cc93e18c0ff624faa0603d61283facc330
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Interfaces;
using LagoVista.IoT.Pipeline.Admin.Interfaces;
using LagoVista.IoT.Pipeline.Admin.Repos;
using LagoVista.IoT.Pipeline.CloudRepos.Repos;

namespace LagoVista.IoT.Pipeline.CloudRepos
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ICustomPipelineConfigurationRepo, CustomPipelineModuleConfigurationRepo>();
            services.AddTransient<IInputTranslatorConfigurationRepo, InputTranslatorConfigurationRepo>();
            services.AddTransient<IListenerConfigurationRepo, ListenerConfigurationRepo>();
            services.AddTransient<IOutputTranslatorConfigurationRepo, OutputTranslatorConfigurationRepo>();
            services.AddTransient<IDataStreamRepo, DataStreamRepo>();
            services.AddTransient<IApplicationCacheRepo, ApplicationCacheRepo>();
            services.AddTransient<ISharedConnectionRepo, SharedDataStreamConnectionRepo>();
            services.AddTransient<ISentinelConfigurationRepo, SentinelConfigurationRepo>();
            services.AddTransient<IPlannerConfigurationRepo, PlannerConfigurationRepo>();
            services.AddTransient<ITransmitterConfigurationRepo, TransmitterConfigurationRepo>();
            services.AddTransient<IPostgresqlServices, PostgresqlServices>();
        }
    }
}
