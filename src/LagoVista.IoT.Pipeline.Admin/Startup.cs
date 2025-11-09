// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: f2417da19929317d05a88420846850fe900bc7935897048a444632a99e68535b
// IndexVersion: 2
// --- END CODE INDEX META ---
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
            services.AddTransient<ISharedConnectionManager, SharedConnectionManager>();
            services.AddTransient<IApplicationCacheManager, ApplicationCacheManager>();

        }
    }
}
