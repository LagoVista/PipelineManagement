using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin.Interfaces;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Pipeline.Admin.Repos;
using LagoVista.IoT.Pipeline.CloudRepos.Repos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

namespace LagoVista.DependencyInjection
{
    public static class PipelineModule
    {
        public static void AddPipelineModule(this IServiceCollection services, IConfigurationRoot configRoot, IAdminLogger logger)
        {
            LagoVista.IoT.Pipeline.CloudRepos.Startup.ConfigureServices(services);
            LagoVista.IoT.Pipeline.Admin.Startup.ConfigureServices(services);
            services.AddMetaDataHelper<DataStream>();
        }
    }
}