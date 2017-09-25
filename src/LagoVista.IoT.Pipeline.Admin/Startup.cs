using LagoVista.IoT.Logging;
using LagoVista.IoT.Pipeline.Admin.Managers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Pipeline.Admin
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            ErrorCodes.Register(typeof(Resources.ErrorCodes));

            services.AddTransient<IPipelineModuleManager, PipelineModuleManager>();
        }
    }
}
