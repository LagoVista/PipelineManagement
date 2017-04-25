using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Pipeline.Admin.Repos;
using LagoVista.IoT.Pipeline.CloudRepos.Repos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

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
            services.AddTransient<ISentinelConfigurationRepo, SentinelConfigurationRepo>();
            services.AddTransient<IPlannerConfigurationRepo, PlannerConfigurationRepo>();
            services.AddTransient<ITransmitterConfigurationRepo, TransmitterConfigurationRepo>();
        }
    }
}
