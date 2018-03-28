using LagoVista.Core.Interfaces;
using LagoVista.IoT.Logging;
using LagoVista.IoT.Pipeline.Admin.Managers;

namespace LagoVista.IoT.Pipeline.Admin
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            ErrorCodes.Register(typeof(Resources.ErrorCodes));

            services.AddTransient<IPipelineModuleManager, PipelineModuleManager>();
            services.AddTransient<IDataStreamManager, DataStreamManager>();

        }
    }
}
